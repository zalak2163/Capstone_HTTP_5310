using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCalendarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventCalendarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EventCalendar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarDto>>> GetEventCalendars()
        {
            var eventCalendars = await _context.EventCalendars
                .Include(e => e.Event) // Include related Event data
                .Include(u => u.User)  // Include related User data
                .Select(e => new CalendarDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    EventId = e.EventId,
                    EventDate = e.EventDate,
                    CreatedAt = e.CreatedAt
                }).ToListAsync();

            return Ok(eventCalendars);
        }

        // GET: api/EventCalendar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarDto>> GetEventCalendar(int id)
        {
            var eventCalendar = await _context.EventCalendars
                .Where(e => e.Id == id)
                .Select(e => new CalendarDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    EventId = e.EventId,
                    EventDate = e.EventDate,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (eventCalendar == null)
            {
                return NotFound();
            }

            return Ok(eventCalendar);
        }

        // POST: api/EventCalendar
        [HttpPost]
        public async Task<ActionResult<CalendarDto>> PostEventCalendar(CalendarDto calendarDto)
        {
            var eventCalendar = new EventCalendar
            {
                UserId = calendarDto.UserId,
                EventId = calendarDto.EventId,
                EventDate = calendarDto.EventDate,
                CreatedAt = DateTime.Now
            };

            _context.EventCalendars.Add(eventCalendar);
            await _context.SaveChangesAsync();

            calendarDto.Id = eventCalendar.Id;

            return CreatedAtAction("GetEventCalendar", new { id = eventCalendar.Id }, calendarDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventCalendar(int id, CalendarDto calendarDto)
        {
            if (id != calendarDto.Id)
            {
                return BadRequest("Event Calendar ID mismatch");
            }

            // Validate UserId exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == calendarDto.UserId);
            if (!userExists)
            {
                return BadRequest("User with the given ID does not exist.");
            }

            // Validate EventId exists
            var eventExists = await _context.Events.AnyAsync(e => e.Id == calendarDto.EventId);
            if (!eventExists)
            {
                return BadRequest("Event with the given ID does not exist.");
            }

            var eventCalendar = await _context.EventCalendars.FindAsync(id);
            if (eventCalendar == null)
            {
                return NotFound();
            }

            // Update properties of the eventCalendar with the data from calendarDto
            eventCalendar.UserId = calendarDto.UserId;
            eventCalendar.EventId = calendarDto.EventId;
            eventCalendar.EventDate = calendarDto.EventDate;

            // Save changes to the database
            _context.Entry(eventCalendar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Status code 204, which means the update was successful but there's no content to return
        }

        // DELETE: api/EventCalendar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventCalendar(int id)
        {
            var eventCalendar = await _context.EventCalendars.FindAsync(id);
            if (eventCalendar == null)
            {
                return NotFound();
            }

            _context.EventCalendars.Remove(eventCalendar);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
