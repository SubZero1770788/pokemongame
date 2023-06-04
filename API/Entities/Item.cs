using System.ComponentModel.DataAnnotations.Schema;
using API.Entities.HelperEntities;

namespace API.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Details { get; set; }
        public List<ItemUser> ItemUsers { get; set; }
    }
}