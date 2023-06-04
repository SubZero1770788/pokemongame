using System.Data;
using System.Net;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _data;

        public ItemsController(DataContext data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Item>>> getItems()
        {
            var items = await _data.Items.ToArrayAsync();

            return items;
        }

        [HttpPost("{id}")]
        public async Task buyItem(ItemBuyDto item)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _data.Users.Where(us => us.UserName == userName).FirstAsync(); 

            var items = _data.Items.AsEnumerable();
            var itemToBuy = items.Where(itb => itb.Id == item.Id).First();

            if(user.Points < itemToBuy.Price* item.amount)
            {
                throw new HttpRequestException("User doesn't have enough points!!");
            } 

            if(await checkIfItemUser(user, itemToBuy, item.amount))
            {
                user.Points -= itemToBuy.Price*item.amount;
                await _data.SaveChangesAsync();
                Ok();
            } 

            user.Points-=itemToBuy.Price*item.amount;
                
            var itemUser = new ItemUser
            {
                ItemId = item.Id,
                UserId = user.Id,
                Amount = item.amount
            };

            _data.ItemUsers.Add(itemUser);
            await _data.SaveChangesAsync();
        }

        private async Task<Boolean> checkIfItemUser(UserData u ,Item i, int amount)
        {
            var userItems = _data.ItemUsers.AsEnumerable();
            foreach(ItemUser ui in userItems)
            {
                if(ui.UserId == u.Id && ui.ItemId == i.Id)
                {
                    var userItem = userItems.Where(uit => uit.ItemId == i.Id && uit.UserId == u.Id).First();
                    userItem.Amount += amount;
                    await _data.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
    }
}