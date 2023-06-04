using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    public class WildPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        // public List<Event> Events { get; set; }
        public ICollection<Pokemon> Pokemons { get; set; }
    }
}