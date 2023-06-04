using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.DTOs.Incoming;
using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class userpokemon : Controller
    {
        private readonly DataContext _data;

        public userpokemon(DataContext data)
        {
            _data = data;
        }

        [HttpPost]
        public async Task<IEnumerable<CurrentUserPokemonDto>> getUserPokemons(PokemonEncounterDto pokemonEncounter)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var currentUser = await this._data.Users.Where(cu => cu.UserName == userName).FirstAsync();

            var currentUserPokemon = await this._data.PokemonUsers.Where(cup => cup.UserId == currentUser.Id && cup.IsInTeam == true).ToListAsync();

            List<CurrentUserPokemonDto> pokemons = new List<CurrentUserPokemonDto>();

            for(int i = 0; i < currentUserPokemon.Count; i++)
            {
                var pokemon = currentUserPokemon[i];
                var pokemonEntity = await this._data.Pokemons.Where(pe => pe.Name == pokemon.Name).FirstAsync();

                var PokemonDtoNum = new CurrentUserPokemonDto
                {
                    Id = currentUserPokemon[i].Id,
                    Name = currentUserPokemon[i].Name,
                    PhotoUrl = pokemonEntity.PhotoUrl,
                    HP = currentUserPokemon[i].HP,
                    Attack = currentUserPokemon[i].Attack,
                    Defense = currentUserPokemon[i].Defense,
                    SpecialAttack = currentUserPokemon[i].SpecialAttack,
                    SpecialDefense = currentUserPokemon[i].SpecialDefense,
                    Speed = currentUserPokemon[i].Speed,
                    level = currentUserPokemon[i].Level,
                    Attack1Id = currentUserPokemon[i].Attack1Id,
                    Attack2Id = currentUserPokemon[i].Attack2Id,
                    Attack3Id = currentUserPokemon[i].Attack3Id,
                    Attack4Id = currentUserPokemon[i].Attack4Id,
                };
                
                pokemons.Add(PokemonDtoNum);
            }

            return pokemons;
        }

        [HttpPost("saveteam")]
        public async Task saveUserPokemon(TeamPokemonDto pid)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(u => u.UserName == userName).FirstAsync();
            var allUserPokemon = await _data.PokemonUsers.Where(pu => pu.UserId == user.Id).ToListAsync();
            var chosenPokemon = allUserPokemon.Where(aup => aup.Id == pid.id).First();
            
            int count = 0;
            for(int i = 0; i < allUserPokemon.Count(); i++)
            {
                if(allUserPokemon[i].IsInTeam == true)
                {
                    count++;
                }
            }

            if(count < 6 && !pid.team)
            {
                chosenPokemon.IsInTeam = true;
            }
            else
            {
                chosenPokemon.IsInTeam = false;
            }

            await _data.SaveChangesAsync();

            await Task.CompletedTask;
        }

        [HttpGet("team")]
        public async Task<IEnumerable<CurrentUserPokemonDto>> getAllUserPokemon()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(u => u.UserName == userName).FirstAsync();
            var allUserPokemon = await _data.PokemonUsers.Where(pu => pu.UserId == user.Id).ToListAsync();

            List<CurrentUserPokemonDto> pokemons = new List<CurrentUserPokemonDto>();

            for(int i = 0; i < allUserPokemon.Count; i++)
            {
                var pokemon = allUserPokemon[i];
                var pokemonEntity = await this._data.Pokemons.Where(pe => pe.Name == pokemon.Name).FirstAsync();

                var PokemonDtoNum = new CurrentUserPokemonDto
                {
                    Id = pokemon.Id,
                    Name = pokemon.Name,
                    PhotoUrl = pokemonEntity.PhotoUrl,
                    HP = pokemon.HP,
                    Attack = pokemon.Attack,
                    Defense = pokemon.Defense,
                    SpecialAttack = pokemon.SpecialAttack,
                    SpecialDefense = pokemon.SpecialDefense,
                    Speed = pokemon.Speed,
                    level = pokemon.Level,
                    Attack1Id = pokemon.Attack1Id,
                    Attack2Id = pokemon.Attack2Id,
                    Attack3Id = pokemon.Attack3Id,
                    Attack4Id = pokemon.Attack4Id,
                    IsInTeam = false
                };

                if(pokemon.IsInTeam)
                {
                    PokemonDtoNum.IsInTeam = true;
                }
                
                pokemons.Add(PokemonDtoNum);
            }

            return pokemons;
        }
    }
}