using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class PokemonPokemonType
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public int PokemonTypeId { get; set; }
    }
}