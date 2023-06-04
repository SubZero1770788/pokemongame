using System.ComponentModel.DataAnnotations.Schema;
using API.Entities.HelperEntities;

namespace API.Entities
{

    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialDefense { get; set; }
        public int SpecialAttack { get; set; }
        public int Speed { get; set; }
        public int BattleGroup { get; set; }
        public string PhotoUrl { get; set; }
        public List<PokemonType> PokemonTypes {get; set;}
        public List<PokemonUser> PokemonUsers { get; set; }
        public int WildPlaceId { get; set; }
        public WildPlace WildPlace { get; set; }
        public List<PokemonAttacks> PokemonAttacks { get; set; }
        public int Attack1Id { get; set; }
        public int Attack2Id { get; set; }
        public int Attack3Id { get; set; }
        public int Attack4Id { get; set; }
    }
}