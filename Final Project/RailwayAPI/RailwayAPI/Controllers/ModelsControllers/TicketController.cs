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
    [Route("[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly railwayContext _context;

        public TicketController(railwayContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> Get()
        {
            var tickets = await _context.Tickets.ToListAsync();

            Response.Headers.Append("X-Total-Count", $"{tickets.Count()}");
            Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
            return tickets;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                return ticket;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Post([FromBody] Ticket value)
        {
            int lastId = Convert.ToInt32(await _context.Tickets.OrderByDescending(p => p.Ticketid).Select(p => p.Ticketid).FirstOrDefaultAsync());

            Ticket newTicket = new Ticket();
            newTicket.Ticketid = lastId + 1;
            newTicket.Seat = value.Seat;
            newTicket.TrainId = value.TrainId;
            newTicket.CarriageId = value.CarriageId;
            newTicket.DepartureDate = value.DepartureDate;
            newTicket.DepartureStationId = value.DepartureStationId;
            newTicket.ArrivalStationId = value.ArrivalStationId;
            newTicket.OperationDate = DateTime.Today;
            newTicket.UserId = value.UserId;
            newTicket.DocumentType = value.DocumentType;
            newTicket.Price = value.Price;
            newTicket.DocumentNumber = value.DocumentNumber;
            newTicket.FirstName = value.FirstName;
            newTicket.LastName = value.LastName;
            newTicket.Status = value.Status;

            var train = await _context.Trains.FindAsync(value.TrainId);
            DateTime thisDay = DateTime.Today;
           
            await _context.AddAsync(newTicket);
            await _context.SaveChangesAsync();
            return newTicket;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}

