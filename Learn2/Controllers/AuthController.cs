using Learn2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            login = login.Trim();
            password = password.Trim();
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

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigManager.AppSetting["JWT:Secret"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(issuer: ConfigManager.AppSetting["JWT:ValidIssuer"],
                        audience: ConfigManager.AppSetting["JWT:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(6),
                        signingCredentials: signinCredentials);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    this.context.UserInfos.Add(new UserInfo { Name = login, Password = PasswordHelper.HashPassword(password), ApiKey = tokenString });
                    this.context.SaveChanges();
                    return Ok(new JWTTokenResponse {
                        Token = tokenString
                    });
                }

            } catch (Exception ex) {
                return StatusCode(404);
            }
            return StatusCode(500);
        }

        [HttpGet(Name = "Register")]
        public IActionResult Register(string login, string password) {
            login = login.Trim();
            password = password.Trim();
            Regex validateNameRegex = new Regex("([a-z]|[A-Z]){3,10}");
            if (!validateNameRegex.IsMatch(password)) {
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
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(issuer: ConfigManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                this.context.UserInfos.Add(new UserInfo { Name = login, Password = PasswordHelper.HashPassword(password), ApiKey = tokenString });
                this.context.SaveChanges();
                return Ok(new JWTTokenResponse {
                    Token = tokenString
                });
            } catch (Exception ex) {
                return StatusCode(500);
            }
        }

    }
}
