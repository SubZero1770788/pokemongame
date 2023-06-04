using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.DTOs.Outgoing;
using API.Entities;
using API.Entities.HelperEntities;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SQLitePCL;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly DataContext _data;

        public PokemonController(DataContext data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Pokemon>>> getPokemon([FromQuery]UserParams userParams)
        {
            var pokemon = _data.Pokemons.AsQueryable();
            var actualPokemon = await PagedList<Pokemon>.CreateAsync(pokemon, userParams.PageNumber, userParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader( 
                actualPokemon.CurrentPage, 
                actualPokemon.PageSize, 
                actualPokemon.TotalCount,
                actualPokemon.TotalPages));

            return Ok(actualPokemon);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<PokemonDto>> getPokemonByPlace(PokemonEncounterDto pokemonEncounter)
        {

            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var thisUser = await _data.Users.Where(un => un.UserName == userName).FirstAsync();

            if (pokemonEncounter.IsFirst == false)
            {
                var encounters = _data.CurrentPokemonEncounter.AsQueryable();
                var encounter = encounters.Where(un => un.UserName == userName);
                _data.CurrentPokemonEncounter.RemoveRange(encounter);
                await _data.SaveChangesAsync();
            }

            if (thisUser.Points == 0)
            {
                return BadRequest("You don't have enough points");
            }
            thisUser.Points--;
            await this._data.SaveChangesAsync();

            var level = thisUser.Level + 2;

            var r = new Random();
            int k = r.Next(0, 100);
            int group = 0;
            group = await checkChance(k);

            var pokemon = await _data.Pokemons.Where(p => p.WildPlaceId == pokemonEncounter.Id
                    && p.BattleGroup == group).OrderBy(p => p.Id).ToArrayAsync();
            int amt = await _data.Pokemons.Where(p => p.WildPlaceId == pokemonEncounter.Id
                    && p.BattleGroup == group).OrderBy(p => p.Id).CountAsync();

            k = r.Next(0, amt);

            var foundPokemon = pokemon[k];
            List<int> types = new List<int>();

            var pokemonTypes = await _data.PokemonPokemonTypes.Where(pt => pt.PokemonId == foundPokemon.Id).ToListAsync();
            foreach(PokemonPokemonType ppt in pokemonTypes)
            {
                types.Add(ppt.PokemonTypeId);
            }

            k = r.Next(level-4, level);

            var randomPokemonAttack = await getAttacks(foundPokemon, k);

            var attacksArray = randomPokemonAttack.ToArray();

            var CurrentPokemonEncounterNew = new CurrentEncounter
            {
                UserName = userName,
                PokemonId = foundPokemon.Id,
                Name = foundPokemon.Name,
                Level = k,
                Attack = foundPokemon.Attack * k,
                Defense = foundPokemon.Defense * k,
                SpecialAttack = foundPokemon.SpecialAttack * k,
                SpecialDefense = foundPokemon.SpecialDefense * k,
                Speed = foundPokemon.Speed * k,
                HP = foundPokemon.HP * k,
                PhotoUrl = foundPokemon.PhotoUrl,
                Attack1Id = attacksArray[0].Id,
                Attack2Id = attacksArray[1].Id,
                Attack3Id = attacksArray[2].Id,
                Attack4Id = attacksArray[3].Id,
            };

            var pokemonDto = new PokemonDto
            {
                Id = foundPokemon.Id,
                Name = foundPokemon.Name,
                Level = k,
                Attack = foundPokemon.Attack * k,
                Defense = foundPokemon.Defense * k,
                SpecialAttack = foundPokemon.SpecialAttack * k,
                SpecialDefense = foundPokemon.SpecialDefense * k,
                Speed = foundPokemon.Speed * k,
                HP = foundPokemon.HP * k,
                PhotoUrl = foundPokemon.PhotoUrl,
                PokemonType1Name = _data.PokemonTypes.Where(pt => pt.Id == types[0]).First().Type,
            };

            if(types.Count > 1)
            {
                pokemonDto.PokemonType2Name = _data.PokemonTypes.Where(pt => pt.Id == types[1]).First().Type;
            }

            _data.CurrentPokemonEncounter.Add(CurrentPokemonEncounterNew);
            await _data.SaveChangesAsync();

            return pokemonDto;
        }

        [HttpPost("catch")]
        public async Task<ActionResult<messageDto>> catchPokemon(ItemBuyDto ItemBuyDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _data.Users.Where(un => un.UserName == userName).First();

            var itemUsers = _data.ItemUsers.AsEnumerable();
            var currentItem = itemUsers.Where(ci => ci.UserId == user.Id && ci.ItemId == ItemBuyDto.Id).First();
            currentItem.Amount--;
            
            var usersPokemon = _data.PokemonUsers.Where(pok => pok.UserId == user.Id).AsEnumerable();
            var pokemonInTeam = usersPokemon.Where(up => up.IsInTeam == true).AsEnumerable();

            foreach(PokemonUser pu in usersPokemon)
            {
                var pokemonHeal = await _data.Pokemons.Where(pok => pok.Id == pu.PokemonId).FirstAsync();
                pu.HP = pokemonHeal.HP*pu.Level;
            }

            if (currentItem.Amount == 0)
            {
                _data.ItemUsers.Remove(currentItem);
            }
            await _data.SaveChangesAsync();

            var currentEncounter = await _data.CurrentPokemonEncounter.Where(un => un.UserName == userName).OrderBy(un => un.Id).LastAsync();

            var isCaught = await tryCatch(currentEncounter, currentItem);

            if (!isCaught)
            {
                _data.CurrentPokemonEncounter.Remove(currentEncounter);
                await _data.SaveChangesAsync();
                var mes1 = new messageDto
                {
                    message = "Pokemon escaped",
                };
                return mes1;
            }

            var pokemon = await _data.Pokemons.Where(p => p.Id == currentEncounter.PokemonId).FirstAsync();
            var random = new Random();

            foreach(PokemonUser pu in usersPokemon)
            {
                if(pu.PokemonId == pokemon.Id)
                {
                    var errorMes = new messageDto
                    {
                        message = "You can't catch more than one of the same pokemon!",
                    };

                    return errorMes;
                }
            }

            var pokemonUser = new PokemonUser
            {
                UserId = user.Id,
                Id = random.Next(),
                PokemonId = currentEncounter.PokemonId,
                Name = pokemon.Name,
                HP = pokemon.HP*currentEncounter.Level,
                Attack = currentEncounter.Attack,
                Defense = currentEncounter.Defense,
                SpecialAttack = currentEncounter.SpecialAttack,
                SpecialDefense = currentEncounter.SpecialDefense,
                Speed = currentEncounter.Speed,
                Level = currentEncounter.Level,
                Attack1Id = currentEncounter.Attack1Id,
                Attack2Id = currentEncounter.Attack2Id,
                Attack3Id = currentEncounter.Attack3Id,
                Attack4Id = currentEncounter.Attack4Id,
                IsInTeam = false
            };

            if(pokemonInTeam.Count() < 6)
            {
                pokemonUser.IsInTeam = true;
            }

            _data.PokemonUsers.Add(pokemonUser);
            await _data.SaveChangesAsync();

            _data.CurrentPokemonEncounter.Remove(currentEncounter);
            await _data.SaveChangesAsync();

            var mes2 = new messageDto
            {
                message = "You did it !!",
            };

            return mes2;
        }
        [HttpGet("pokedex/{id}")]
        public async Task<PokedexPokemonDto> getPokemon(int id)
        {
            var pokemon = await _data.Pokemons.Where(pok => pok.Id == id).FirstAsync();
            var WildPlace = await _data.WildPlaces.Where(wp => wp.Id == pokemon.WildPlaceId).FirstAsync();

            List<string> pokemonTypes = new List<string>();
            List<string> pokemonAttacks = new List<string>();
            var pokemonpokemontypes = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == pokemon.Id).AsEnumerable();
            var nextPokemon = await _data.Pokemons.Where(pok => pok.Id == pokemon.Id + 1).FirstAsync();
            int level = 0;
            if(pokemon.Level >= 1 && nextPokemon.Level > 1)
            {
                level = nextPokemon.Level;
            }
            foreach (PokemonPokemonType pt in pokemonpokemontypes)
            {
                var type = await _data.PokemonTypes.Where(ppt => ppt.Id == pt.PokemonTypeId).FirstAsync();
                pokemonTypes.Add(type.Type);
            }

            var Attacks = _data.PokemonAttacks.Where(patt => patt.PokemonsId == pokemon.Id).AsEnumerable();
            foreach (PokemonAttacks a in Attacks)
            {
                var Attack = await _data.Attacks.Where(att => att.Id == a.AttacksId).FirstAsync();
                pokemonAttacks.Add(Attack.AttackName);
            }

            if (pokemonTypes.Count() > 1)
            {
                var newPokemon2 = new PokedexPokemonDto
                {
                    Id = pokemon.Id,
                    Name = pokemon.Name,
                    Level = level,
                    HP = pokemon.HP,
                    Attack = pokemon.Attack,
                    Defense = pokemon.Defense,
                    SpecialAttack = pokemon.SpecialAttack,
                    SpecialDefense = pokemon.SpecialDefense,
                    Speed = pokemon.Speed,
                    PhotoUrl = pokemon.PhotoUrl,
                    WildPlace = WildPlace.Name,
                    PokemonType1 = pokemonTypes[0],
                    PokemonType2 = pokemonTypes[1],
                    Attacks = pokemonAttacks,
                };
                return newPokemon2;
            }
            var newPokemon = new PokedexPokemonDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Level = level,
                HP = pokemon.HP,
                Attack = pokemon.Attack,
                Defense = pokemon.Defense,
                SpecialAttack = pokemon.SpecialAttack,
                SpecialDefense = pokemon.SpecialDefense,
                Speed = pokemon.Speed,
                PhotoUrl = pokemon.PhotoUrl,
                WildPlace = WildPlace.Name,
                PokemonType1 = pokemonTypes[0],
                Attacks = pokemonAttacks,
            };
            return newPokemon;
        }

        private async Task<int> checkChance(int i)
        {
            int returnable = 0;
            switch (i)
            {
                case int n when (n <= 40):
                    returnable = 1;
                    break;
                case int n when (n <= 65):
                    returnable = 2;
                    break;
                case int n when (n <= 80):
                    returnable = 3;
                    break;
                case int n when (n <= 95):
                    returnable = 4;
                    break;
                case int n when (n > 95):
                    returnable = 5;
                    break;
            }
            return returnable;
        }

        private async Task<bool> tryCatch(CurrentEncounter ce, ItemUser ci)
        {
            int pokemonId = ce.PokemonId;
            var pokemon = await _data.Pokemons.Where(pk => pk.Id == pokemonId).FirstAsync();
            int chance = 2;

            var items = _data.Items.AsEnumerable();
            var item = items.Where(it => it.Id == ci.ItemId).First();

            switch (item.Name)
            {
                case "Greatball":
                    chance = 4;
                    break;
                case "Ultraball":
                    chance = 9;
                    break;
                case "Masterball":
                    chance = 100;
                    return true;
            }

            var ran = new Random();

            int catchChance = ran.Next(1, 10);
            chance = chance * catchChance;
            int pokemonStrength = 1;

            switch (pokemon.BattleGroup)
            {
                case 1:
                    pokemonStrength = ran.Next(1, 20);
                    break;
                case 2:
                    pokemonStrength = ran.Next(1, 45);
                    break;
                case 3:
                    pokemonStrength = ran.Next(15, 65);
                    break;
                case 4:
                    pokemonStrength = ran.Next(35, 90);
                    break;
                case 5:
                    pokemonStrength = ran.Next(60, 100);
                    break;
            }

            if (chance >= pokemonStrength)
            {
                return true;
            }
            return false;
        }

        private async Task<List<Attack>> getAttacks(Pokemon p, int level)
        {
            var attacks = await _data.PokemonAttacks.Where(a => a.PokemonsId == p.Id).ToArrayAsync();

            List<Attack> moveset = new List<Attack>();

            int blockedTypeId = 0;

            for (int i = attacks.Length - 1; i >= 0; i--)
            {
                int attackJunction = attacks[i].AttacksId;

                var actualAttack = await _data.Attacks.Where(aa => aa.Id == attackJunction).FirstAsync();

                if (moveset.Count >= 2 && blockedTypeId == 0)
                {
                    Attack a1 = moveset[0];
                    Attack a2 = moveset[1];

                    if (a1.PokemonTypeId == a2.PokemonTypeId)
                    {
                        blockedTypeId = a1.PokemonTypeId;
                    }
                }

                if (actualAttack.Level <= level && actualAttack.PokemonTypeId != blockedTypeId)
                {
                    moveset.Add(actualAttack);
                }
            };

            if (moveset.Count() < 4)
            {
                var struggle = await _data.Attacks.Where(a => a.AttackName == "Struggle").FirstAsync();

                for (int i = moveset.Count - 1; i < 4; i++)
                {
                    moveset.Add(struggle);
                }
            }
            return moveset;
        }

    }
}