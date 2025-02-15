using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _context.Tickets
                                         .Select(t => new TicketDto
                                         {
                                             Id = t.Id,
                                             TicketType = t.TicketType,
                                             Price = t.Price,
                                             Quantity = t.Quantity,
                                             Available = t.Available,
                                             CreatedAt = t.CreatedAt,
                                             EventId = t.EventId
                                         })
                                         .ToListAsync();

            return Ok(tickets);
        }

        // GET api/ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                                        .Where(t => t.Id == id)
                                        .Select(t => new TicketDto
                                        {
                                            Id = t.Id,
                                            TicketType = t.TicketType,
                                            Price = t.Price,
                                            Quantity = t.Quantity,
                                            Available = t.Available,
                                            CreatedAt = t.CreatedAt,
                                            EventId = t.EventId
                                        })
                                        .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // POST api/ticket
        [HttpPost]
        public async Task<ActionResult<TicketDto>> PostTicket(TicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                TicketType = ticketDto.TicketType,
                Price = ticketDto.Price,
                Quantity = ticketDto.Quantity,
                Available = ticketDto.Available,
                CreatedAt = DateTime.UtcNow,
                EventId = ticketDto.EventId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            ticketDto.Id = ticket.Id;
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticketDto);
        }

        // PUT api/ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketDto ticketDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.TicketType = ticketDto.TicketType;
            ticket.Price = ticketDto.Price;
            ticket.Quantity = ticketDto.Quantity;
            ticket.Available = ticketDto.Available;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
