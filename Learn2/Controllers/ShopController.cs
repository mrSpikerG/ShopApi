using Learn2.Cache;
using Learn2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn2.Controllers {
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShopController : ControllerBase {

        private readonly ICacheService CacheService;
        private readonly SHOPContext Context;


        public ShopController(ICacheService cacheService) {
            this.Context = new SHOPContext();
            this.CacheService = cacheService;
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
        public async Task<ActionResult<IEnumerable<ShopItem>>> Read() {

            List<ShopItem> productsCache = this.CacheService.GetData<List<ShopItem>>("ShopItem");
            if (productsCache == null) {
                var productSQL = await this.Context.ShopItems.ToListAsync();
                if (productSQL.Count > 0) {
                    this.CacheService.SetData("Product", productSQL, DateTimeOffset.Now.AddDays(1));
                    return productSQL;
                }
            }
            return productsCache;
        }

        [HttpPost(Name = "UpdateItem")]
        public IActionResult Update([FromQuery] int id, [FromQuery] string newName, [FromQuery] decimal newPrice) {
            if (!this.Context.ShopItems.Any(x => x.Id == id)) {
                return StatusCode(404);
            }

   

            if (newPrice <= 0) {
                return StatusCode(406);
            }

            ShopItem item = this.Context.ShopItems.First(x => x.Id == id);
         

            item.Name = newName;
            item.Price = newPrice;
            this.Context.SaveChanges();
            return Ok();
        }

        [HttpPost(Name = "DeleteItem")]
        public IActionResult Delete([FromQuery]int id) {

            

            if (!this.Context.ShopItems.Any(x => x.Id==id)) {
                return StatusCode(404);
            }

            ShopItem item = this.Context.ShopItems.First(x => x.Id == id);
            
            this.Context.Remove(item);
            this.Context.SaveChanges();
            return Ok();
        }
    }
}