using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.HelperEntities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class BaseController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly IToken _tokenS;
        private readonly UserManager<UserData> _userManager;

        public BaseController(UserManager<UserData> userManager, DataContext data, IToken tokenS)
        {
            _data = data;
            _tokenS = tokenS;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserLocalDataDto>> Register(UserDataRegisterDto userRegister)
        {
            if (await checkUser(userRegister)) return BadRequest("Username is already taken !");

            var pokemon = await searchPokemon(userRegister);

            var user = new UserData
            {
                UserName = userRegister.UserName,
                Level = 5,
                Points = 100
            };

            var r = await _userManager.CreateAsync(user, userRegister.Password);

            if(!r.Succeeded) return BadRequest(r.Errors);

            var pokemonuser = await setStarter(pokemon, user);

            var userLocal = new UserLocalDataDto
          {
            Level = user.Level,
            Points = user.Points,
            Token = _tokenS.CreateToken(user)
          };

            return userLocal;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLocalDataDto>> loginUser(UserDataLoginDto userLogin)
        {
          var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == userLogin.username);

          if (user == null) return Unauthorized("User doesn't exist");

          var r = await _userManager.CheckPasswordAsync(user, userLogin.password);

          if(!r) return Unauthorized("Bad password");

            var userLocal = new UserLocalDataDto
          {
            Level = user.Level,
            Token = _tokenS.CreateToken(user),
            Points = user.Points
          };

          return userLocal;
        }

        private async Task<bool> checkUser(UserDataRegisterDto name)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == name.UserName.ToLower());
        }

        private async Task<PokemonUser> setStarter(Entities.Pokemon p, UserData user)
        {
            var pokemonAttacks = _data.PokemonAttacks.Where(x => x.PokemonsId == p.Id).AsEnumerable();

            var Random = new Random();

            var pokemonUser = new PokemonUser
            {
                Id = Random.Next(),
                UserId = user.Id,
                PokemonId = p.Id,
                Name = p.Name,
                HP = p.HP*5,
                Attack = p.Attack*5,
                Defense = p.Defense*5,
                SpecialAttack = p.SpecialAttack*5,
                SpecialDefense = p.SpecialDefense*5,
                Speed = p.Speed*5,
                Experience = 0,
                Level = 5,
                Attack1Id = 14,
                Attack2Id = 210,
                Attack3Id = 210,
                Attack4Id = 210,
                IsInTeam = true,
            };

            _data.PokemonUsers.Add(pokemonUser);
            await _data.SaveChangesAsync();

            return pokemonUser;
        }

        private async Task<Entities.Pokemon> searchPokemon(UserDataRegisterDto userDataRegister)
        {
            var pokemon = await _data.Pokemons.Where(p => p.Name.ToLower() == userDataRegister.Pokemon.ToLower()).FirstOrDefaultAsync();

            return pokemon;
        }
    }

}