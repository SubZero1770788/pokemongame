using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class pokeAttack
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public int AttackId { get; set; }
    }
}