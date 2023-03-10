using Learn2.Cache;
using Learn2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn2.Controllers {

    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CategoryController : Controller {

        private readonly ICacheService CacheService;
        private readonly SHOPContext Context;
        public CategoryController(ICacheService cacheService) {
            this.Context = new SHOPContext();
            this.CacheService = cacheService;
        }

        [HttpPost(Name = "CreateCategory")]
        public IActionResult CreateCategory(string name) {
            try {
                this.Context.Categories.Add(new Category() { Name = name });
                this.Context.SaveChanges();
                return Ok();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return StatusCode(406);
            }
        }

       

        [HttpGet(Name = "ReadCategory")]
        public async Task<ActionResult<IEnumerable<Category>>> ReadCategory() {

            List<Category> Cache = this.CacheService.GetData<List<Category>>("ShopItem");
            if (Cache == null) {
                var productSQL = await this.Context.Categories.ToListAsync();
                if (productSQL.Count > 0) {
                    this.CacheService.SetData("Product", productSQL, DateTimeOffset.Now.AddDays(1));
                    return productSQL;
                }
            }
            return Cache;
        }



        [HttpPost(Name = "UpdateCategory")]
        public IActionResult UpdateCategory([FromQuery] int id, [FromQuery] string newName) {
            if (!this.Context.Categories.Any(x => x.Id == id)) {
                return StatusCode(404);
            }



            Category item = this.Context.Categories.First(x => x.Id == id);
            item.Name = newName;

            this.Context.SaveChanges();
            return Ok();
        }

        [HttpPost(Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int id) {


            if (!this.Context.ShopItems.Any(x => x.Id == id)) {
                return StatusCode(404);
            }

            if (this.Context.CategoryConnections.Any(x => x.CategoryId == id)) {
                var forDelete = this.Context.CategoryConnections.ToList().FindAll(x => x.CategoryId == id);
                this.Context.CategoryConnections.RemoveRange(forDelete);
            }
            this.Context.Remove(this.Context.ShopItems.First(x => x.Id == id));
            this.Context.SaveChanges();
            return Ok();
        }

 
    }
}
