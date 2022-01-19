using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayAPI.Controllers
{
    [Route("lists")]
    [ApiController]
    public class ListForFrontController : Controller
    {
        private readonly railwayContext _context;

        public ListForFrontController(railwayContext context)
        {
            _context = context;
        }
        [HttpGet("employeeslist")]
        public async Task<ActionResult> GetEmployees()
        {
            var emplyees = await _context.Employees.ToListAsync();
            var result = emplyees.Select(r => new { id = r.Id.ToString(), name = r.LastName });
            return Ok(result);
        }

        [HttpGet("trainslist")]
        public async Task<ActionResult> GetTrains()
        {
            var trains = await _context.Trains.ToListAsync();
            var result = trains.Select(r => new { id = r.Id.ToString(), name = r.Id.ToString() });
            return Ok(result);
        }

        [HttpGet("routeslist")]
        public async Task<ActionResult> GetRoutes()
        {
            var routes = await _context.Routes.ToListAsync();
            var result = routes.Select(r => new { id = r.Id.ToString(), name = $"{r.FirstStationId}-{r.LastStationId}" });
            return Ok(result);
        }

        [HttpGet("carriageslist")]
        public async Task<ActionResult> GetCarriages()
        {
            var carriages = await _context.Carriages.ToListAsync();
            var result = carriages.Select(r => new { id = r.Id.ToString(), name = r.Id.ToString() });
            return Ok(result);
        }

        [HttpGet("stationslist")]
        public async Task<ActionResult> GetStations()
        {
            var stations = await _context.Stations.ToListAsync();
            var result = stations.Select(r => new { id = r.Id.ToString(), name = r.Name.ToString() });
            return Ok(result);
        }
    }
}
