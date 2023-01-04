using Learn2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Learn2.Controllers {
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : Controller {


        private SHOPContext context;
        public AuthController() {
            this.context = new SHOPContext();
        }


        [HttpGet(Name = "Login")]
        public IActionResult Login(string login, string password) {
            Regex validateNameRegex = new Regex("([a-z]|[A-Z]){3,10}");
            if (!validateNameRegex.IsMatch(password)) {
                return StatusCode(403);
            }

            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$");
            if (!validateGuidRegex.IsMatch(password)) {
                return StatusCode(403);
            }

            try {
                UserInfo user = this.context.UserInfos.First(x => x.Name.Equals(login));
                if (user.Password.Equals(PasswordHelper.HashPassword(password))) {
                    return Ok(user.ApiKey);
                }

            } catch (Exception ex) {
                return StatusCode(404);
            }
            return StatusCode(500);
        }

        [HttpGet(Name = "Register")]
        public IActionResult Register(string login, string password) {
            Regex validateNameRegex = new Regex("([a-z]|[A-Z]){3,10}");
            if(!validateNameRegex.IsMatch(password)) {
                return StatusCode(403);
            }

            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$");
            if (!validateGuidRegex.IsMatch(password)) {
                return StatusCode(403);
            }


            if (this.context.UserInfos.Any(x => x.Name.Equals(login))) {
                return StatusCode(403);
            }

            try {
                this.context.UserInfos.Add(new UserInfo { Name = login, Password = PasswordHelper.HashPassword(password), ApiKey = PasswordHelper.generateAPIKey() });
                this.context.SaveChanges();
                return Ok();
            }catch(Exception ex) {
                return StatusCode(500);
            }
        }

    }
}
