
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RailwayAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly railwayContext _context;
        public UserController(railwayContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _context.Users.ToListAsync();

            Response.Headers.Append("X-Total-Count", $"{ users.Count()}");
            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"user 1-10/{users.Count()}");
            return users;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if ( user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            int lastId = Convert.ToInt32(await _context.Users.OrderByDescending(p => p.id).Select(p => p.id).FirstOrDefaultAsync());

            User newUser = new User();
            newUser.id = lastId + 1;
            newUser.Email = user.Email;
            newUser.Password = user.Password;
            newUser.Role = user.Role;
            newUser.Blocked = user.Blocked;

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return Created("success", newUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put([FromBody] User value, int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Password = value.Password;
                user.Role = value.Role;
                user.Blocked = value.Blocked;

                await _context.SaveChangesAsync();
                return user;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
