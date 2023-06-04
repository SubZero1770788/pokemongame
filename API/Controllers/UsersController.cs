using API.Data;
using API.DTOs.Outgoing;
using API.Entities;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<userDataDto>>> getUsers([FromQuery]UserParams userParams)
        {
            var users = _context.Users.AsQueryable();

            var actualUsers = await PagedList<userDataDto>.ReturnRanking(users, userParams.PageNumber, userParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(
                actualUsers.CurrentPage,
                actualUsers.PageSize,
                actualUsers.TotalCount,
                actualUsers.TotalPages));


            return Ok(actualUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserData>> getUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            
            return user;
        }
    }
}