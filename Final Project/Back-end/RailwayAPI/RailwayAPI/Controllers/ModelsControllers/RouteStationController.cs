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
    public class RouteStationController : Controller
    {
        private readonly railwayContext _context;
        public RouteStationController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        { 
            var result = from a in _context.RouteStations
                         select new
                         {
                             a.Id,
                             a.StationId,
                             a.Order,
                             ArrivalTime = a.ArrivalTime.ToString(@"HH\:mm"),
                             DepartureTime = a.DepartureTime.ToString(@"HH\:mm")

                         };


            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"route-stations 1-10/{result.Count()}");
            return Ok(result);
        }

        [HttpGet("{station},{route},{order}")]
        public async Task<ActionResult> Get(string station, int route, int order)
        {
            var routeStations = await _context.RouteStations.ToListAsync();
            RouteStation routeStation = null;

            for (int a = 0; a < routeStations.Count; a++)
            {
                if (routeStations[a].Id == route && routeStations[a].StationId == station && routeStations[a].Order == order)
                {
                    routeStation = routeStations[a];
                }
            }

            if (routeStation == null)
            {
                return NotFound();
            }
            else
            {
                var result = new
                {
                    routeStation.Id,
                    routeStation.StationId,
                    routeStation.Order,
                    ArrivalTime = routeStation.ArrivalTime.ToString(@"HH\:mm"),
                    DepartureTime = routeStation.DepartureTime.ToString(@"HH\:mm")
                };
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult<RouteStation>> Post([FromBody] RouteStation value)
        {
            var routeStations = await _context.RouteStations.ToListAsync();
            RouteStation routeStation = null;

            for (int a = 0; a < routeStations.Count; a++)
            {
                if (routeStations[a].Id == value.Id && routeStations[a].StationId == value.StationId && routeStations[a].Order == value.Order)
                {
                    routeStation = routeStations[a];
                }
            }

            if (routeStation != null)
            {
                return BadRequest();
            }
            else
            {
                RouteStation newRS = new RouteStation();
                newRS.Id = value.Id;
                newRS.Order = value.Order;
                newRS.StationId = value.StationId;
                newRS.ArrivalTime = value.ArrivalTime.ToLocalTime(); ;
                newRS.DepartureTime = value.DepartureTime.ToLocalTime();
                var data = await _context.RouteStations.ToListAsync();
                var route = await _context.Routes.FindAsync(value.Id);
                int maxOrder = data.Where(p => p.Id == value.Id).Max(a => a.Order);

                if (value.Order == 1)
                {

                    route.DepartureTime = value.DepartureTime;
                }
                else if (value.Order == maxOrder + 1)
                {
                    route.ArrivalTime = value.ArrivalTime;
                }

                await _context.AddAsync(newRS);
                await _context.SaveChangesAsync();

                return newRS;
            }

        }

        [HttpPut("{station},{route},{order}")]
        public async Task<ActionResult<RouteStation>> Put([FromBody] RouteStation value)
        {
            var routeStations = await _context.RouteStations.ToListAsync();
            RouteStation routeStation = null;

            for (int a = 0; a < routeStations.Count; a++)
            {
                if (routeStations[a].Id == value.Id && routeStations[a].StationId == value.StationId && routeStations[a].Order == value.Order)
                {
                    routeStation = routeStations[a];
                }
            }

            if (routeStation == null)
            {
                return NotFound();
            }
            else
            {
                routeStation.ArrivalTime = value.ArrivalTime.AddMinutes(3).ToLocalTime();
                routeStation.DepartureTime = value.DepartureTime.AddMinutes(3).ToLocalTime();
                var data = await _context.RouteStations.ToListAsync();
                var trains = await _context.Trains.ToListAsync();
                int maxOrder = data.Where(p => p.Id == value.Id).Max(a => a.Order);
                var route = await _context.Routes.FindAsync(value.Id);

                if (value.Order == 0)
                {
                    route.DepartureTime = value.DepartureTime;

                }
                else if (value.Order == maxOrder)
                {
                    route.ArrivalTime = value.ArrivalTime;
                }

                await _context.SaveChangesAsync();
                return routeStation;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, string stationId, int order)
        {
            var routeStations = await _context.RouteStations.ToListAsync();
            RouteStation routeStation = null;

            for (int a = 0; a < routeStations.Count; a++)
            {
                if (routeStations[a].Id == id && routeStations[a].StationId == stationId && routeStations[a].Order == order)
                {
                    routeStation = routeStations[a];
                }
            }

            if (routeStation == null)
            {
                return NotFound();
            }

            var routes = await (_context.RouteStations.Where(a => a.Id == id && a.Order > order)).ToListAsync();


            if ((routes != null) && (routes.Any()))
            {
                foreach (var rs in routes)
                {
                    _context.RouteStations.Remove(rs);
                }

                await _context.SaveChangesAsync();

                foreach (var rs in routes)
                {
                    rs.Order--;
                    await _context.RouteStations.AddAsync(rs);
                }

            }
            else 
            {
                var route = await _context.Routes.FindAsync(id);
                var lastRS = await (_context.RouteStations.Where(a => a.Id == id && a.Order == order - 1)).ToListAsync();
                route.LastStationId = lastRS[0].StationId;

            }
            _context.RouteStations.Remove(routeStation);
            await _context.SaveChangesAsync();
            return NoContent();
            
        }
    }
}
