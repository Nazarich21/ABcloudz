using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RailwayAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationController : Controller
    {
        private readonly railwayContext _context;
        public StationController(railwayContext context)
        {
            _context = context;
        }
   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> Get()
        {
            var stations = await _context.Stations.ToListAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"station 1-10/{stations.Count()}");

            return stations;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Get(string id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }
            else
            {
                return station;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Station>> Post([FromBody] Station value)
        {
            Station station = await _context.Stations.FindAsync(value.Id);

            if (station != null)
            {
                return Conflict();
            }
            else
            {
                Station newStation = new Station();
                newStation.Id = value.Id;
                newStation.Name = value.Name;
                _context.Add(newStation);
                await _context.SaveChangesAsync();
                return newStation;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Station>> Put([FromBody] Station value, string id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }
            else
            {
                station.Name = value.Name;
                await _context.SaveChangesAsync();
                return station;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }
            else
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
