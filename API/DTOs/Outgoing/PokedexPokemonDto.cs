using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Outgoing
{
    public class PokedexPokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialDefense { get; set; }
        public int SpecialAttack { get; set; }
        public int Speed { get; set; }
        public string PhotoUrl { get; set; }
        public string PokemonType1 { get; set; }
        public string? PokemonType2 { get; set; }
        public string WildPlace { get; set; }
        public List<string> Attacks { get; set; }
    }
}