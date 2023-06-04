using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class Ineffective
    {
        public int Id { get; set; }
        public int IneffectiveId { get; set; }
        public int IneffectiveTypeId { get; set; }
        public IneffectiveTypes IneffectiveTypes { get; set; }
        public PokemonType PokemonType { get; set; }
    }
}