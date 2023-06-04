using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialDefense { get; set; }
        public int SpecialAttack { get; set; }
        public int Speed { get; set; }
        public string PhotoUrl { get; set; }
        public int PokemonTypeId1 { get; set; }
        public string PokemonType1Name { get; set; }
        public int? PokemonTypeId2 { get; set; }
        public string? PokemonType2Name { get; set; }
        public int WildPlaceId { get; set; }
        public int Attack1Id { get; set; }
        public string Attack1Name { get; set; }
        public string Attack1Type { get; set; }
        public int Attack2Id { get; set; }
        public string Attack2Name { get; set; }
        public string Attack2Type { get; set; }
        public int Attack3Id { get; set; }
        public string Attack3Name { get; set; }
        public string Attack3Type { get; set; }
        public int Attack4Id { get; set; }
        public string Attack4Name { get; set; }
        public string Attack4Type { get; set; }
    }
}