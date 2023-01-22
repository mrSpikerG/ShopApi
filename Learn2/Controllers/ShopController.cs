using Learn2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn2.Controllers {
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShopController : ControllerBase {


        private readonly SHOPContext Context;


        public ShopController() {
            this.Context = new SHOPContext();
        }

        [HttpPost(Name = "CreateItem")]
        public IActionResult Create(ItemModal itemModal) {
            try {

                if (!this.Context.UserInfos.Any(x => x.ApiKey.Equals(itemModal.ApiKey))) {
                    return StatusCode(404);
                }

                if (itemModal.Price <= 0) {
                    return StatusCode(406);
                }

                
                this.Context.ShopItems.Add(new ShopItem() { Name = itemModal.Name, Price = itemModal.Price, UserId= this.Context.UserInfos.First(x => x.ApiKey.Equals(itemModal.ApiKey)).Id });
                this.Context.SaveChanges();

                return Ok();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return StatusCode(406);
            }
        }

        [HttpGet(Name = "ReadItem")]
        public IEnumerable<ShopItem> Read() {
            return this.Context.ShopItems;
        }

        [HttpPost(Name = "UpdateItem")]
        public IActionResult Update([FromQuery] int id, [FromQuery] string newName, [FromQuery] decimal newPrice, [FromQuery] string apiKey) {
            if (!this.Context.ShopItems.Any(x => x.Id == id)) {
                return StatusCode(404);
            }

            if (!this.Context.UserInfos.Any(x => x.ApiKey.Equals(apiKey))) {
                return StatusCode(404);
            }

            if (newPrice <= 0) {
                return StatusCode(406);
            }

            ShopItem item = this.Context.ShopItems.First(x => x.Id == id);
            if(!this.Context.UserInfos.First(x => x.Id == item.UserId).ApiKey.Equals(apiKey)) {
                return StatusCode(403);
            }

            item.Name = newName;
            item.Price = newPrice;
            this.Context.SaveChanges();
            return Ok();
        }

        [HttpPost(Name = "DeleteItem")]
        public IActionResult Delete([FromQuery]int id, [FromQuery] string apiKey) {

            if (!this.Context.UserInfos.Any(x => x.ApiKey.Equals(apiKey))) {
                return StatusCode(404);
            }

            if (!this.Context.ShopItems.Any(x => x.Id==id)) {
                return StatusCode(404);
            }

            ShopItem item = this.Context.ShopItems.First(x => x.Id == id);
            if (!this.Context.UserInfos.First(x => x.Id == item.UserId).ApiKey.Equals(apiKey)) {
                return StatusCode(403);
            }

            this.Context.Remove(item);
            this.Context.SaveChanges();
            return Ok();
        }
    }
}