using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class PokemonAttacks
    {
        public int Id { get; set; }
        public int AttacksId { get; set; }
        public int PokemonsId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Attack Attack { get; set; }
    }
}