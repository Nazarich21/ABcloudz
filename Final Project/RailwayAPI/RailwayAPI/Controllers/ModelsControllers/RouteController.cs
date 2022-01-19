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
    public class RouteController : Controller
    {
        private readonly railwayContext _context;

        public RouteController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  ActionResult Get()
        {
            var result = from a in _context.Routes
                         select new
                         {
                             a.Id,
                             a.FirstStationId,
                             ArrivalTime = a.ArrivalTime.ToString(@"HH\:mm"),
                             a.LastStationId,
                             DepartureTime = a.DepartureTime.ToString(@"HH\:mm")

                         };

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"trains 1-10/{result.Count()}");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }
            else
            {

                return new { route.Id,
                             route.FirstStationId, 
                             ArrivalTime = route.ArrivalTime.ToString(@"HH\:mm"),
                             route.LastStationId, DepartureTime = route.DepartureTime.ToString(@"HH\:mm") 
                           };
            }
        }

        [HttpPost]
        public async Task<ActionResult<Route>> Post([FromBody] Route value)
        {
            int lastId = Convert.ToInt32(await _context.Routes.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefaultAsync());

            Route newRoute = new Route();
            newRoute.Id = lastId + 1;
            newRoute.FirstStationId = value.FirstStationId;
            newRoute.LastStationId = value.LastStationId;
            newRoute.ArrivalTime = value.ArrivalTime.ToLocalTime();
            newRoute.DepartureTime = value.DepartureTime.ToLocalTime();

            await _context.AddAsync(newRoute);
            await _context.SaveChangesAsync();
            return newRoute;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Route>> Put([FromBody] Route value, int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }
            else
            {
                route.FirstStationId = value.FirstStationId;
                route.LastStationId = value.LastStationId;
                route.ArrivalTime = value.ArrivalTime.AddMinutes(2).ToLocalTime();
                route.DepartureTime = value.DepartureTime.AddMinutes(2).ToLocalTime();
                await _context.SaveChangesAsync();
                return route;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            int trains = await (_context.Trains.Where( a => a.RouteId == id)).CountAsync();
            if (route == null)
            {
                return NotFound();
            } 
            else if ( trains != 0)
            {
                return Conflict();
            }
            else
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
