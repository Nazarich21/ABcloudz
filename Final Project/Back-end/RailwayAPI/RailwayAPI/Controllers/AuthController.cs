using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RailwayAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly railwayContext _context;

        public AuthController(railwayContext context)
        {
            _context = context;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            var identity = GetIdentity(login.Email, login.Password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
           
            Response.Cookies.Append("jwt", encodedJwt, new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
            User user = FindUser(login.Email, login.Password).Result;
            return Ok(user);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = FindUser(email, password).Result;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("id",user.id.ToString()),
                    new Claim("role", user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        private async Task<User> FindUser(string email, string password) 
        {
            User user = await _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            return user;
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            var userCheck = await _context.Users.Where(p => p.Email == user.Email).FirstOrDefaultAsync();

            if (userCheck != null)
            {
                return Conflict("Email Duplicate");
            }
            else
            {
                int lastId = Convert.ToInt32(await _context.Users.OrderByDescending(p => p.id).Select(p => p.id).FirstOrDefaultAsync());
                User add = new User();
                add.id = lastId + 1;
                add.Email = user.Email;
                add.Password = user.Password;

                if (user.Role == null)
                {
                    add.Role = "user";
                }
                else
                {
                    add.Role = user.Role;
                }

                add.Blocked = false;

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("id",add.id.ToString()),
                    new Claim("role", add.Role)
                };

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                          issuer: AuthOptions.ISSUER,
                          audience: AuthOptions.AUDIENCE,
                          notBefore: now,
                          claims: claims,
                          expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                          signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                  var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                Response.Cookies.Append("jwt", encodedJwt, new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
                await _context.AddAsync(add);
                await _context.SaveChangesAsync();
                return Created("success", add);
            }
        }

        [Authorize]
        [Route("userget")]
        [HttpGet]
        public async Task<ActionResult> FindUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string id = identity.FindFirst("Id").Value;
            User user = await _context.Users.Where(u => u.id == Int32.Parse(id)).FirstOrDefaultAsync();
            return Ok(user);
          
        }
        [Route("logout")]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
          
            return Ok("Succesfull");
        }
    }
}
