using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.DTOs.Incoming;
using API.DTOs.Outgoing;
using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BattleController : ControllerBase
    {
        private readonly DataContext _data;
        public BattleController(DataContext data)
        {
            _data = data;
        }

        [HttpPost]
        public async Task<IEnumerable<PokemonDto>> pokemonBattle(PokemonEncounterDto sentPokemon)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(u => u.UserName == userName).FirstAsync();
            var userPokemons = _data.PokemonUsers.Where(pu => pu.UserId == user.Id).AsEnumerable();
            var chosen = userPokemons.Where(ch => ch.Id == sentPokemon.Id).First();
            var userPokemon = _data.Pokemons.Where(pk => pk.Id == chosen.PokemonId).First();
            var userTypes = await _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == userPokemon.Id).ToListAsync();
            var attack1 = _data.Attacks.Where(att => att.Id == chosen.Attack1Id).First();
            var attack2 = _data.Attacks.Where(att => att.Id == chosen.Attack2Id).First();
            var attack3 = _data.Attacks.Where(att => att.Id == chosen.Attack3Id).First();
            var attack4 = _data.Attacks.Where(att => att.Id == chosen.Attack4Id).First();
            var attack1Encounter = _data.Attacks.Where(att => att.Id == chosen.Attack1Id).First();
            var attack2Encounter = _data.Attacks.Where(att => att.Id == chosen.Attack2Id).First();
            var attack3Encounter = _data.Attacks.Where(att => att.Id == chosen.Attack3Id).First();
            var attack4Encounter = _data.Attacks.Where(att => att.Id == chosen.Attack4Id).First();

            var currentEncounter = await _data.CurrentPokemonEncounter.Where(cpe => cpe.UserName == user.UserName).OrderBy(cpe => cpe.Id).LastAsync();
            var Encounterpokemon = await _data.Pokemons.Where(pok => pok.Id == currentEncounter.PokemonId).FirstAsync();
            var Encountertypes = await _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == currentEncounter.PokemonId).ToListAsync();

            List<int> userPptype = new List<int>();
            List<int> encounterPptype = new List<int>();
            List<PokemonDto> pokemonDtos = new List<PokemonDto>();

            for (int i = 0; i < Encountertypes.Count(); i++)
            {
                encounterPptype.Add(Encountertypes[i].PokemonTypeId);
            }

            for (int i = 0; i < userTypes.Count(); i++)
            {
                userPptype.Add(userTypes[i].PokemonTypeId);
            }

            var pokemonEncounter = new PokemonDto
            {
                Id = currentEncounter.Id,
                Name = currentEncounter.Name,
                Level = currentEncounter.Level,
                HP = currentEncounter.HP,
                Attack = currentEncounter.Attack,
                Defense = currentEncounter.Defense,
                SpecialAttack = currentEncounter.SpecialAttack,
                SpecialDefense = currentEncounter.SpecialDefense,
                Speed = currentEncounter.Speed,
                PhotoUrl = currentEncounter.PhotoUrl,
                PokemonTypeId1 = encounterPptype[0],
                PokemonType1Name = _data.PokemonTypes.Where(pt => pt.Id == encounterPptype[0]).First().Type,
                Attack1Id = chosen.Attack1Id,
                Attack1Name = attack1Encounter.AttackName,
                Attack2Id = chosen.Attack2Id,
                Attack2Name = attack2Encounter.AttackName,
                Attack3Id = chosen.Attack3Id,
                Attack3Name = attack3Encounter.AttackName,
                Attack4Id = chosen.Attack4Id,
                Attack4Name = attack4Encounter.AttackName,
            };

            if (encounterPptype.Count() > 1)
            {
                pokemonEncounter.PokemonTypeId2 = encounterPptype[1];
                pokemonEncounter.PokemonType2Name = _data.PokemonTypes.Where(pt => pt.Id == encounterPptype[1]).First().Type;
            }

            var userPokemonDto = new PokemonDto
            {
                Id = chosen.Id,
                Name = chosen.Name,
                Level = chosen.Level,
                Experience = chosen.Experience,
                HP = chosen.HP,
                Attack = chosen.Attack,
                Defense = chosen.Defense,
                SpecialAttack = chosen.SpecialAttack,
                SpecialDefense = chosen.SpecialDefense,
                Speed = chosen.Speed,
                PhotoUrl = userPokemon.PhotoUrl,
                PokemonTypeId1 = userPptype[0],
                PokemonType1Name = _data.PokemonTypes.Where(pt => pt.Id == userPptype[0]).First().Type,
                Attack1Id = chosen.Attack1Id,
                Attack1Name = attack1.AttackName,
                Attack1Type = _data.PokemonTypes.Where(pt => pt.Id == attack1.PokemonTypeId).First().Type,
                Attack2Id = chosen.Attack2Id,
                Attack2Name = attack2.AttackName,
                Attack2Type = _data.PokemonTypes.Where(pt => pt.Id == attack2.PokemonTypeId).First().Type,
                Attack3Id = chosen.Attack3Id,
                Attack3Name = attack3.AttackName,
                Attack3Type = _data.PokemonTypes.Where(pt => pt.Id == attack3.PokemonTypeId).First().Type,
                Attack4Id = chosen.Attack4Id,
                Attack4Name = attack4.AttackName,
                Attack4Type = _data.PokemonTypes.Where(pt => pt.Id == attack4.PokemonTypeId).First().Type,
            };

            if (userPptype.Count() > 1)
            {
                userPokemonDto.PokemonTypeId2 = userPptype[1];
                userPokemonDto.PokemonType2Name = _data.PokemonTypes.Where(pt => pt.Id == userPptype[1]).First().Type;
            }

            pokemonDtos.Add(pokemonEncounter);
            pokemonDtos.Add(userPokemonDto);
            return pokemonDtos;
        }

        [HttpPost("attackChange")]
        public async Task<messageDto> changeAttack(AttackDto change)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(us => us.UserName == userName).FirstAsync();
            var userPokemon = await _data.PokemonUsers.Where(poku => poku.UserId == user.Id && poku.Id == change.PokemonId).FirstAsync();
            var pokemon = await _data.Pokemons.Where(pok => pok.Id == userPokemon.PokemonId).FirstAsync();
            List<int> currentAttacks = new List<int>();
            currentAttacks.Add(userPokemon.Attack1Id);
            currentAttacks.Add(userPokemon.Attack2Id);
            currentAttacks.Add(userPokemon.Attack3Id);
            currentAttacks.Add(userPokemon.Attack4Id);
            var PokemonAttacksJunction = _data.PokemonAttacks.Where(pokatt => pokatt.PokemonsId == pokemon.Id).AsEnumerable();
            var message = new messageDto
            {
                message = "Your pokemon already knows this attack !!",
            };
            foreach (PokemonAttacks pa in PokemonAttacksJunction)
            {
                var Attack = await _data.Attacks.Where(att => att.Id == pa.AttacksId).FirstAsync();
                if (Attack.Level == userPokemon.Level)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentAttacks[i] == change.ChosenAttackId)
                        {
                            if (currentAttacks.Contains(pa.Id)) return message;
                            currentAttacks[i] = Attack.Id;
                            message.AttackId = Attack.Id;
                            message.AttackName = Attack.AttackName;
                            message.message = "Your pokemon learned a new Attack:";
                            break;
                        }
                    }
                }
            }
            userPokemon.Attack1Id = currentAttacks[0];
            userPokemon.Attack2Id = currentAttacks[1];
            userPokemon.Attack3Id = currentAttacks[2];
            userPokemon.Attack4Id = currentAttacks[3];
            await _data.SaveChangesAsync();
            return message;
        }

        [HttpPost("{id}")]
        public async Task<messageDto> gameLoop(AttackDto chosenAttack)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(us => us.UserName == userName).FirstAsync();
            var pokemon = await _data.PokemonUsers.Where(pok => pok.Id == chosenAttack.PokemonId && pok.UserId == user.Id).FirstAsync();
            var attackTarget = await _data.CurrentPokemonEncounter.Where(cpe => cpe.UserName == userName).OrderBy(cpe => cpe.Id).LastAsync();
            var Message = new messageDto
            {
                message = ""
            };

            int attackId = 0;
            if (chosenAttack.ChosenAttackId == pokemon.Attack1Id) attackId = pokemon.Attack1Id;
            if (chosenAttack.ChosenAttackId == pokemon.Attack2Id) attackId = pokemon.Attack2Id;
            if (chosenAttack.ChosenAttackId == pokemon.Attack3Id) attackId = pokemon.Attack3Id;
            if (chosenAttack.ChosenAttackId == pokemon.Attack4Id) attackId = pokemon.Attack4Id;
            if (attackId == 0)
            {
                Message.message = "Invalid attack used !!";
                return Message;
            }
            var attackData = _data.Attacks.Where(att => att.Id == attackId).First();

            if (pokemon.Speed >= attackTarget.Speed)
            {
                await calculateDamage(attackData, chosenAttack, pokemon, attackTarget);
                if (attackTarget.HP <= 0)
                {
                    await restoreHP(pokemon, attackTarget, Message);
                    Message.message = "Victory!!";
                    return Message;
                }
                int a = await calculateOponentDamage(pokemon, attackTarget);
                await dealOpponentDamage(a, pokemon);
                if (pokemon.HP <= 0)
                {
                    await restoreHP(pokemon, attackTarget, Message);
                    Message.message = "Defeat!!";
                    return Message;
                }
            }
            else
            {
                int a = await calculateOponentDamage(pokemon, attackTarget);
                await dealOpponentDamage(a, pokemon);
                if (pokemon.HP <= 0)
                {
                    await restoreHP(pokemon, attackTarget, Message);
                    Message.message = "Defeat!!";
                    return Message;
                }
                await calculateDamage(attackData, chosenAttack, pokemon, attackTarget);
                if (attackTarget.HP <= 0)
                {
                    await restoreHP(pokemon, attackTarget, Message);
                    Message.message = "Victory!!";
                    return Message;
                }
            }
            Message.message = "Attack hit !";
            Message.enemyCurrentHp = attackTarget.HP;
            Message.userCurrentHp = pokemon.HP;
            return Message;
        }

        [HttpPost("heal/{id}")]
        public async Task<messageDto> healPokemon(UsingItemDto uiDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(us => us.UserName == userName).FirstAsync();
            var pokemonTarget = await _data.PokemonUsers.Where(pt => pt.Id == uiDto.PokemonId && pt.UserId == user.Id).FirstAsync();
            var userItem = await _data.ItemUsers.Where(itu => itu.ItemId == uiDto.ItemId && itu.UserId == user.Id).FirstAsync();
            var actualItem = await _data.Items.Where(it => it.Id == userItem.ItemId).FirstAsync();

            if(userItem.Amount == 1)
            {
                _data.ItemUsers.Remove(userItem);
            }
            userItem.Amount--;
            await _data.SaveChangesAsync();

            switch(actualItem.Id)
            {
                case 5:
                   await healUser(5, pokemonTarget);
                break;
                case 6:
                    await healUser(6, pokemonTarget);
                break;
                case 7:
                    await healUser(7, pokemonTarget);
                break;
            }

            var m = new messageDto{
                message = "Your pokemon was healed !!",
                userCurrentHp = pokemonTarget.HP,
            };
            
            return m;
        }

        private async Task healUser(int i, PokemonUser pu)
        {
            var pokemonTemplate = await _data.Pokemons.Where(pok => pok.Id == pu.PokemonId).FirstAsync();
            switch(i)
            {
                case 5:
                    pu.HP +=1000;
                break;
                case 6:
                    pu.HP +=2500;
                break;
                case 7:
                    pu.HP +=10000;
                break;
            }

            if(pu.HP > pokemonTemplate.HP*pu.Level)
            {
                pu.HP = pokemonTemplate.HP*pu.Level;
            }

            await _data.SaveChangesAsync();
        }
        
        private async Task restoreHP(PokemonUser p, CurrentEncounter ce, messageDto m)
        {
            if (p.HP > 0)
            {
                await gainExperience(p, ce, m);
            }
            var actualUserPokemon = await _data.Pokemons.Where(pok => pok.Id == p.PokemonId).FirstAsync();
            p.HP = actualUserPokemon.HP * p.Level;
            var encounterPokemon = await _data.Pokemons.Where(pok => pok.Id == ce.PokemonId).FirstAsync();
            ce.HP = encounterPokemon.HP * ce.Level;
            _data.CurrentPokemonEncounter.Remove(ce);
            await _data.SaveChangesAsync();
        }

        private async Task<ActionResult<messageDto>> gainExperience(PokemonUser p, CurrentEncounter ce, messageDto m)
        {
            var encounterPokemon = await _data.Pokemons.Where(pok => pok.Id == ce.PokemonId).FirstAsync();
            int experience = encounterPokemon.BattleGroup;
            var pokemon = await _data.Pokemons.Where(pok => pok.Id == p.PokemonId).FirstAsync();
            p.Experience += experience;

            if (p.Experience >= 5)
            {
                p.Experience = 0;
                p.Level++;
                p.Attack = pokemon.Attack * p.Level;
                p.Defense = pokemon.Defense * p.Level;
                p.SpecialAttack = pokemon.SpecialAttack * p.Level;
                p.SpecialDefense = pokemon.SpecialDefense * p.Level;
                p.Speed = pokemon.Speed * p.Level;
                var user = await _data.Users.Where(us => us.Id == p.UserId).FirstAsync();
                if (p.Level > user.Level)
                {
                    user.Level++;
                }
                await doesEvolve(p);
                var AttackP = await getAttack(p, m);
            }
            await _data.SaveChangesAsync();
            return m;
        }

        private async Task<ActionResult<messageDto>> getAttack(PokemonUser p, messageDto m)
        {
            var AttacksJunction = _data.PokemonAttacks.Where(pokatt => pokatt.PokemonsId == p.PokemonId).AsEnumerable();

            foreach (PokemonAttacks a in AttacksJunction)
            {
                var Attack = await _data.Attacks.Where(att => att.Id == a.AttacksId).FirstAsync();
                if (Attack.Level == p.Level)
                {
                    m.AttackId = Attack.Id;
                    m.AttackName = Attack.AttackName;
                    break;
                }
            }
            return m;
        }

        private async Task doesEvolve(PokemonUser p)
        {
            var pokemon = await _data.Pokemons.Where(pok => pok.Id == p.PokemonId).FirstAsync();
            var nextPokemon = await _data.Pokemons.Where(pok => pok.Id == pokemon.Id + 1).FirstAsync();

            if (p.Level >= nextPokemon.Level && nextPokemon.Level > 1)
            {
                var evolution = new PokemonUser
                {
                    Id = p.Id + 1,
                    Attack = nextPokemon.Attack * p.Level,
                    Defense = nextPokemon.Defense * p.Level,
                    SpecialAttack = nextPokemon.SpecialAttack * p.Level,
                    SpecialDefense = nextPokemon.SpecialDefense * p.Level,
                    Speed = nextPokemon.Speed * p.Level,
                    Name = nextPokemon.Name,
                    Level = p.Level,
                    HP = nextPokemon.HP * p.Level,
                    Attack1Id = p.Attack1Id,
                    Attack2Id = p.Attack2Id,
                    Attack3Id = p.Attack3Id,
                    Attack4Id = p.Attack4Id,
                    Experience = 0,
                    PokemonId = nextPokemon.Id,
                    UserId = p.UserId,
                    Pokemon = nextPokemon,
                };
                _data.PokemonUsers.Remove(p);
                await _data.SaveChangesAsync();
                evolution.Id--;
                _data.PokemonUsers.Add(evolution);
            }
        }

        private async Task dealOpponentDamage(int a, PokemonUser p)
        {
            p.HP -= a;
            await _data.SaveChangesAsync();
        }
        private async Task<int> calculateOponentDamage(PokemonUser p, CurrentEncounter ce)
        {

            var Random = new Random();
            var attackNum = Random.Next(1, 4);
            List<int> opponentAttacks = new List<int>();
            opponentAttacks.Add(ce.Attack1Id);
            opponentAttacks.Add(ce.Attack2Id);
            opponentAttacks.Add(ce.Attack3Id);
            opponentAttacks.Add(ce.Attack4Id);
            var attack = await _data.Attacks.Where(at => at.Id == opponentAttacks[attackNum]).FirstAsync();
            attackNum = Random.Next(1, 101);
            if (attackNum < attack.Accuracy)
            {
                var attackType = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonTypeId == attack.PokemonTypeId).First();
                var encounterType = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == p.PokemonId).AsEnumerable();
                var userPokemon = await _data.Pokemons.Where(pok => pok.Id == p.PokemonId).FirstAsync();
                var userPokemonTypes = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == p.PokemonId).AsEnumerable();
                var type1 = userPokemonTypes.First();
                var ineffective = _data.Ineffectives.AsEnumerable();
                var weaks1 = _data.Weaks.Where(w => w.WeakId == type1.PokemonTypeId);
                var resistsType1 = _data.Resistants.Where(r => r.ResistantId == type1.PokemonTypeId);
                int target = 0;
                int hit = 0;
                if (attack.Split == false)
                {
                    hit = ce.Attack;
                    target = p.Defense;
                }
                else
                {
                    hit = ce.SpecialAttack;
                    target = p.SpecialDefense;
                }

                double damage = 2 * ce.Level / 5 + 2;
                damage *= attack.AttackPower;
                damage *= (double)hit / (double)target;
                damage /= 4;

                foreach (var ineff in ineffective)
                {
                    if (ineff.IneffectiveId == attackType.PokemonTypeId && ineff.IneffectiveTypeId == type1.PokemonTypeId)
                    {
                        damage = 0;
                        int zero = 0;
                        return zero;
                    }
                }

                foreach (var w1 in weaks1)
                {
                    if (w1.WeakId == attackType.Id)
                    {
                        damage *= 2;
                        break;
                    }
                }

                foreach (var r1 in resistsType1)
                {
                    if (r1.ResistantTypeId == attackType.Id)
                    {
                        damage *= 0.5;
                        break;
                    }
                }

                if (userPokemonTypes.Count() > 1)
                {
                    var type2 = userPokemonTypes.Last();
                    var weaksType2 = _data.Weaks.Where(we => we.WeakTypeId == type2.PokemonTypeId).AsEnumerable();
                    var resistsType2 = _data.Resistants.Where(we => we.ResistantId == type2.PokemonTypeId).AsEnumerable();

                    foreach (var ineff in ineffective)
                    {
                        if (ineff.IneffectiveId == attackType.PokemonTypeId && ineff.IneffectiveTypeId == type2.PokemonTypeId)
                        {
                            damage = 0;
                            int zero = 0;
                            return zero;
                        }
                    }
                    foreach (var w2 in weaksType2)
                    {
                        if (w2.WeakId == attackType.Id)
                        {
                            damage *= 2;
                            break;
                        }
                    }

                    foreach (var r2 in resistsType2)
                    {
                        if (r2.ResistantTypeId == attackType.Id)
                        {
                            damage *= 0.5;
                            break;
                        }
                    }
                }

                foreach (var type in encounterType)
                {
                    if (type.PokemonTypeId == attack.PokemonTypeId)
                    {
                        damage *= 1.5;
                    }
                }

                return (int)Math.Floor(damage);
            }
            return 0;
        }
        private async Task calculateDamage(Attack attackData, AttackDto chosenAttack, PokemonUser p, CurrentEncounter attackTarget)
        {
            var pokemon = await _data.Pokemons.Where(pok => pok.Id == p.PokemonId).FirstAsync();
            var random = new Random();
            int evasion = random.Next(1, 101);
            Boolean notMissed = true;
            if (attackData.Accuracy < evasion)
            {
                notMissed = false;
            }
            if (notMissed)
            {

                bool split = attackData.Split;
                int target = 0;
                int hit = 0;

                if (split)
                {
                    target = attackTarget.SpecialDefense;
                    hit = p.SpecialAttack;
                }
                else
                {
                    target = attackTarget.Defense;
                    hit = p.Attack;
                }

                double damage = 2 * (double)(p.Level / 5) + 2;
                damage *= attackData.AttackPower;
                damage *= (double)hit / (double)target;
                damage /= 4;

                var targetTypes = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == attackTarget.PokemonId).AsEnumerable();
                var userTypes = _data.PokemonPokemonTypes.Where(ppt => ppt.PokemonId == p.PokemonId).AsEnumerable();
                foreach (var ut in userTypes)
                {
                    if (ut.PokemonTypeId == attackData.PokemonTypeId)
                    {
                        damage *= 1.5;
                        break;
                    }
                }

                List<int> types = new List<int>();

                foreach (var t in targetTypes)
                {
                    types.Add(t.PokemonTypeId);
                }

                int damageDealt = await getDamage(types, damage, attackData);
                attackTarget.HP -= damageDealt;
                await _data.SaveChangesAsync();
            };
        }

        private async Task<int> getDamage(List<int> types, double damage, Attack attackData)
        {
            var attackType = _data.PokemonTypes.Where(pt => pt.Id == attackData.PokemonTypeId).First();
            var ineffective = await _data.Ineffectives.ToArrayAsync();
            var type1 = types[0];
            var weaksType1 = _data.Weaks.Where(we => we.WeakId == type1).AsEnumerable();
            var resistsType1 = _data.Resistants.Where(we => we.ResistantId == type1).AsEnumerable();

            for (int i = 0; i < ineffective.Length; i++)
            {
                if (attackType.Id == ineffective[i].IneffectiveId && ineffective[i].IneffectiveTypeId == type1)
                {
                    damage = 0;
                    int zero = 0;
                    return zero;
                }
            }

            foreach (var w1 in weaksType1)
            {
                if (w1.WeakTypeId == attackType.Id)
                {
                    damage *= 2;
                    break;
                }
            }

            foreach (var r1 in resistsType1)
            {
                if (r1.ResistantTypeId == attackType.Id)
                {
                    damage *= 0.5;
                    break;
                }
            }

            if (types.Count() > 1)
            {
                var type2 = types[1];
                var weaksType2 = _data.Weaks.Where(we => we.WeakId == type2).AsEnumerable();
                var resistsType2 = _data.Resistants.Where(we => we.ResistantId == type2).AsEnumerable();

                for (int i = 0; i < ineffective.Length; i++)
                {
                    if (attackType.Id == ineffective[i].IneffectiveId && ineffective[i].IneffectiveTypeId == type2)
                    {
                        damage = 0;
                        int zero = 0;
                        return zero;
                    }
                }

                foreach (var w2 in weaksType2)
                {
                    if (w2.WeakTypeId == attackType.Id)
                    {
                        damage *= 2;
                        break;
                    }
                }

                foreach (var r2 in resistsType2)
                {
                    if (r2.ResistantTypeId == attackType.Id)
                    {
                        damage *= 0.5;
                        break;
                    }
                }
            }

            int roundedDamage = (int)Math.Floor(damage);

            return roundedDamage;
        }
    }
}