using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WildPlacesController : ControllerBase
    {
        private readonly DataContext _data;
        public WildPlacesController(DataContext data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<WildPlace>>> getPlaces()
        {
            var places = _data.WildPlaces.ToArrayAsync();

            return await places;
        }
    }
}