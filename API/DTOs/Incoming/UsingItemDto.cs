using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Incoming
{
    public class UsingItemDto
    {
        public int ItemId { get; set; }
        public int PokemonId { get; set; }
    }
}