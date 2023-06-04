using System.ComponentModel.DataAnnotations.Schema;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class UserData : IdentityUser<int>
    {
        public int Points { get; set; }
        public int Level { get; set; }
        public string PhotoUrl { get; set; }
        public List<PokemonUser> Pokemons { get; set; }
        public List<ItemUser> ItemUsers { get; set; }
        public ICollection<UserDataUserRole> UserRoles { get; set; }
    }
}