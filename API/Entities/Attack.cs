using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Attack
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string AttackName { get; set; }
        public int AttackPower { get; set; }
        public int Accuracy { get; set; }
        public bool Split { get; set; }
        public int PokemonTypeId { get; set; }
        public PokemonType PokemonType { get; set; }
        public List<PokemonAttacks> PokemonAttacks { get; set; }
    }
}