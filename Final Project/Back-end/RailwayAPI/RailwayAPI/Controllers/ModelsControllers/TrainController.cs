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
    public class TrainController : Controller
    {
        private readonly railwayContext _context;
        public TrainController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> Get()
        {
            var trains = await _context.Trains.ToListAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"trains 1-10/{trains.Count()}");
            return trains;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Train>> Get(int id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
            {
                return NotFound();
            }
            else
            {
                return train;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Train>> Post([FromBody] Train value)
        {
            int lastId = Convert.ToInt32(await _context.Trains.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefaultAsync());

            Train newTrain = new Train();
            newTrain.Id = lastId + 1;
            newTrain.RouteId = value.RouteId;
            newTrain.DriverId = value.DriverId;
            newTrain.DriverAssistId = value.DriverAssistId;
            newTrain.TrainmasterId = value.TrainmasterId;
            newTrain.Type = value.Type;
            newTrain.EveryDay = value.EveryDay;
            newTrain.EvenDays = value.EvenDays;

            if (value.Days != null)
            {
                newTrain.Days = value.Days;
            }

            await _context.AddAsync(newTrain);
            await _context.SaveChangesAsync();
            return newTrain;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Train>> Put([FromBody] Train value, int id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
            {
                return NotFound();
            }
            else
            {
                train.RouteId = value.RouteId;
                train.DriverId = value.DriverId;
                train.DriverAssistId = value.DriverAssistId;
                train.TrainmasterId = value.TrainmasterId;
                train.Type = value.Type;
                train.EveryDay = value.EveryDay;
                train.EvenDays = value.EvenDays;

                if (value.Days != null)
                {
                    train.Days = value.Days;
                }

                await _context.SaveChangesAsync();
                return train;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            var carriages = await (_context.Carriages.Where(a => a.TrainId == id)).CountAsync();

            if (train == null)
            {
                return NotFound();
            }
            else if (carriages != 0)
            {
                return Conflict();
            }
            else
            {
                _context.Remove(train);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
