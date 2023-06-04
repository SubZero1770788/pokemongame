using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class Weak
    {
        public int Id { get; set; }
        public int WeakId { get; set; }
        public int WeakTypeId { get; set; }
        public WeakTypes WeakTypes { get; set; }
        public PokemonType PokemonType { get; set; }
    }
}