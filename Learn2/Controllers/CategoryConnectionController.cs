using Learn2.Cache;
using Learn2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn2.Controllers {

    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryConnectionController : Controller {
        
        private readonly SHOPContext Context;
        private readonly ICacheService CacheService;

        public CategoryConnectionController(ICacheService cacheService) {
            this.Context = new SHOPContext();
            this.CacheService = cacheService;
        }

        [HttpPost(Name = "AddItemToCategory")]
        public IActionResult AddConnection(int CategoryId, int ItemId) {
            if (!this.Context.Categories.Any(x => x.Id == CategoryId) || !this.Context.ShopItems.Any(x => x.Id == ItemId)) {
                return StatusCode(404);
            }

            this.Context.CategoryConnections.Add(new CategoryConnection() { CategoryId = CategoryId, PhoneId = ItemId });
            this.Context.SaveChanges();
            return Ok();
        }

      
        [HttpGet(Name = "ReadCategoryConn")]
        public async Task<ActionResult<IEnumerable<CategoryConnection>>> ReadCategory() {

            List<CategoryConnection> Cache = this.CacheService.GetData<List<CategoryConnection>>("ShopItem");
            if (Cache == null) {
                var productSQL = await this.Context.CategoryConnections.ToListAsync();
                if (productSQL.Count > 0) {
                    this.CacheService.SetData("Product", productSQL, DateTimeOffset.Now.AddDays(1));
                    return productSQL;
                }
            }
            return Cache;
        }


        [HttpPost(Name = "RemoveItemToCategory")]
        public IActionResult RemoveConnection(int id) {
            if (this.Context.CategoryConnections.Any(x => x.Id == id)) {
                return StatusCode(404);
            }

            this.Context.CategoryConnections.ToList().RemoveAll(x => x.Id == id);
            this.Context.SaveChanges();
            return Ok();
        }

    }
}
