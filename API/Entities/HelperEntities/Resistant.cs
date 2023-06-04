using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class Resistant
    {
        public int Id { get; set; }
        public int ResistantId { get; set; }
        public int ResistantTypeId { get; set; }
        public PokemonType PokemonType { get; set; }
        public ResistantTypes ResistantTypes { get; set; }
    }
}