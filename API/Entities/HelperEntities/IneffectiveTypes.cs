using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.HelperEntities
{
    public class IneffectiveTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Ineffective> Ineffectives { get; set; }
    }
}