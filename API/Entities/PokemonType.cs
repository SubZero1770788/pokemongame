using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.HelperEntities;

namespace API.Entities
{
    public class PokemonType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Ineffective> Ineffective { get; set; }
        public List<Resistant> Resistant { get; set; }
        public List<Weak> Weaks { get; set; }
        public List<Pokemon> Pokemons { get; set; }
        public List<Attack> Attacks { get; set; }
    }
}