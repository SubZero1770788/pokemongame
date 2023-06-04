using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Outgoing
{
    public class messageDto
    {
        public string message { get; set; }
        public int userCurrentHp { get; set; }
        public int enemyCurrentHp { get; set; }
        public int? AttackId { get; set; }
        public string? AttackName { get; set; }
    }
}