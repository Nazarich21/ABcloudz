using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RailwayAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SeatController : Controller
    {
        private readonly railwayContext _context;

        public SeatController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> Get()
        {
            var seats = await _context.Seats.ToListAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"carriage 1-10/{seats.Count()}");
            return seats;
        }

        [HttpGet("{id},{no}")]
        public async Task<ActionResult<Seat>> Get(int id, string no)
        {
            var seats = await _context.Seats.ToListAsync(); ;
            Seat seat = new Seat();

            for (int a = 0; a < seats.Count; a++)
            {
                if (seats[a].SeatNo == no && seats[a].Id == id)
                {
                    seat = seats[a];
                }
            }

            if (seat == null)
            {
                return NotFound();
            }
            else
            {
                return seat;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Seat>> Post([FromBody] Seat value)
        {
            var seats = await _context.Seats.ToListAsync();
            Seat seat = null;

            for (int a = 0; a < seats.Count; a++)
            {
                if (seats[a].SeatNo == value.SeatNo && seats[a].Id == value.Id)
                {
                    seat = seats[a];
                }
            }

            if (seat == null)
            {
                Seat newSeat = new Seat();
                newSeat.SeatNo = value.SeatNo;
                newSeat.Id = value.Id;
                newSeat.Status = value.Status;
                await _context.AddAsync(newSeat);
                await _context.SaveChangesAsync();
                return newSeat;
            }
            else
            {
                return Conflict();
            }

        }

        [HttpPut("{id},{no}")]
        public async Task<ActionResult<Seat>> Put([FromBody] Seat value, int id, string no)
        {
            var seats = await _context.Seats.ToListAsync();
            Seat seat = null;

            for (int a = 0; a < seats.Count; a++)
            {
                if (seats[a].SeatNo == no && seats[a].Id == id)
                {
                    seat = seats[a];
                }
            }

            if (seat == null)
            {
                return NotFound();
            }
            else
            {
                seat.Status = value.Status;
                await _context.SaveChangesAsync();
                return seat;
            }
        }

        [HttpDelete("{id},{no}")]
        public async Task<IActionResult> Delete(string no, int id)
        {
            var seats = await _context.Seats.ToListAsync();
            Seat seat = null;
            var currentDate = (DateTime.Now).Date;

            for (int a = 0; a < seats.Count; a++)
            {
                if (seats[a].SeatNo == no && seats[a].Id == id)
                {
                    seat = seats[a];
                }
            }
            var tickets = await( _context.Tickets.Where(a => a.Seat == no && a.CarriageId == id && a.DepartureDate < currentDate)).CountAsync();

            if (seat == null)
            {
                return NotFound();
            }
            else if (tickets != 0)
            {
                return Conflict();
            }
            else
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
                return Ok(seat);
            }
        }
    }
}
