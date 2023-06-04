using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.HelperEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class userItems : ControllerBase
    {
        private readonly DataContext _data;

        public userItems(DataContext data)
        {
            _data = data;
        }

        [HttpPost]
        public async Task<IEnumerable<ItemDto>> getUserItems(ItemBuyDto itemBuyDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var currentUser = await this._data.Users.Where(cu => cu.UserName == userName).FirstAsync();

            List<ItemDto> ReturnableUserItems = new List<ItemDto>();

            if(await checkIfUserHasItems(currentUser))
            {
                var userItems = await this._data.ItemUsers.Where(iu => iu.UserId == currentUser.Id).ToArrayAsync();

                foreach(ItemUser ui in userItems)
                {
                    var currentItem = await this._data.Items.Where(it => it.Id == ui.ItemId).FirstAsync();
                    var UseritemDto = new ItemDto
                    {
                        name = currentItem.Name,
                        ItemId = currentItem.Id,
                        PhotoUrl = currentItem.PhotoUrl,
                        Amount = ui.Amount,
                        Price = currentItem.Price,
                    };

                    ReturnableUserItems.Add(UseritemDto);
                }
            }
            return ReturnableUserItems; 
        }

        private async Task<Boolean> checkIfUserHasItems(UserData u)
        {
            var userItems = _data.ItemUsers.AsEnumerable();
            foreach(ItemUser ui in userItems)
            {
                if(ui.UserId == u.Id)
                {
                   return true;
                }
            }

            return false;
        }
    }
}