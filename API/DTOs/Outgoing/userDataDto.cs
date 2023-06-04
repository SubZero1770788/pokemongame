using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Outgoing
{
    public class userDataDto
    {
        public string UserName { get; set; }
        public int level { get; set; }
        public int points { get; set; }
    }
}