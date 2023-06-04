using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class WeakTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Weak> Weaks{ get; set; }
    }
}