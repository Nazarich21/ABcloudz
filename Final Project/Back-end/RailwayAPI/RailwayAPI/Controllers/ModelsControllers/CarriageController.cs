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
    public class CarriageController : Controller
    {
        private readonly railwayContext _context;

        public CarriageController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carriage>>> Get()
        {
            var carriages = await _context.Carriages.ToListAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"carriage 1-10/{carriages.Count()}");
            return carriages;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carriage>> Get(int id)
        {
            var carriage = await _context.Carriages.FindAsync(id);

            if (carriage == null)
            {
                return NotFound();
            }
            else
            {
                return carriage;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Carriage>> Post([FromBody] Carriage value)
        {
            int lastId = Convert.ToInt32(await _context.Carriages.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefaultAsync());

            Carriage add = new Carriage();
            add.Id = lastId + 1;
            add.TrainId = value.TrainId;
            add.Type = value.Type;
            add.Guide1Id = value.Guide1Id;
            add.Guide2Id = value.Guide2Id;

            await _context.AddAsync(add);
            await _context.SaveChangesAsync();
            return add;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Carriage>> Put([FromBody] Carriage value, int id)
        {
            var carriage = await _context.Carriages.FindAsync(id);

            if (carriage == null)
            {
                return NotFound();
            }
            else
            {
                carriage.TrainId = value.TrainId;
                carriage.Type = value.Type;
                carriage.Guide1Id = value.Guide1Id;
                carriage.Guide2Id = value.Guide2Id;
                await _context.SaveChangesAsync();
                return carriage;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var carriage = await _context.Carriages.FindAsync(id);
            var seats = await ( _context.Seats.Where( a => a.Id == id)).CountAsync();

            if (carriage == null)
            {
                return NotFound();
            }
            else if (seats != 0)
            {
                return Conflict();
            }
            else
            {
                _context.Carriages.Remove(carriage);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
