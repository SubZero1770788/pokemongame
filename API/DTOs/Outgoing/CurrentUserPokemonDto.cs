using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CurrentUserPokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public int level { get; set; }
        public int Attack1Id { get; set; }
        public int Attack2Id { get; set; }
        public int Attack3Id { get; set; }
        public int Attack4Id { get; set; }
        public bool? IsInTeam { get; set; }
    }
}