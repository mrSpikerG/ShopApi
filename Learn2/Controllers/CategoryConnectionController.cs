using Learn2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learn2.Controllers {

    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryConnectionController : Controller {
        private readonly SHOPContext Context;
        public CategoryConnectionController() {
            this.Context = new SHOPContext();
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
        public IEnumerable<CategoryConnection> ReadCategory() {
            return this.Context.CategoryConnections;
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
