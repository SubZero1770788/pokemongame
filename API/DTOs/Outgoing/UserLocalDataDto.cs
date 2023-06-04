using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.HelperEntities;

namespace API.DTOs
{
    public class UserLocalDataDto
    {
        public int Level { get; set; }
        public int Points { get; set; }
        public string Token { get; set; }
    }
}