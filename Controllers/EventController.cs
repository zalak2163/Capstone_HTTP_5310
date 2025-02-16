using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                                        .Include(e => e.Organizer)  // Eagerly load the Organizer
                                        .Select(e => new EventDto
                                        {
                                            Id = e.Id,
                                            Title = e.Title,
                                            Description = e.Description,
                                            EventDate = e.EventDate,
                                            Location = e.Location,
                                            Image = e.Image,
                                            CreatedAt = e.CreatedAt,
                                            OrganizerName = e.Organizer != null ? e.Organizer.Name : "Unknown",
                                            OrganizerId = e.OrganizerId
                                        })
                                        .ToListAsync();

            return Ok(events);
        }

        // GET api/event/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventEntity = await _context.Events
                                            .Include(e => e.Organizer)  // Eagerly load the Organizer
                                            .Where(e => e.Id == id)
                                            .Select(e => new EventDto
                                            {
                                                Id = e.Id,
                                                Title = e.Title,
                                                Description = e.Description,
                                                EventDate = e.EventDate,
                                                Location = e.Location,
                                                Image = e.Image,
                                                CreatedAt = e.CreatedAt,
                                                OrganizerName = e.Organizer != null ? e.Organizer.Name : "Unknown",  // Dynamically fetch OrganizerName
                                                OrganizerId = e.OrganizerId
                                            })
                                            .FirstOrDefaultAsync();

            if (eventEntity == null)
            {
                return NotFound();
            }

            // Debugging: Log the OrganizerName and OrganizerId being returned
            Console.WriteLine($"Returning Event {eventEntity.Id} with OrganizerName: {eventEntity.OrganizerName} and OrganizerId: {eventEntity.OrganizerId}");

            return Ok(eventEntity);
        }

        // POST api/event
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            // Check if the organizer exists in the database (if OrganizerId is provided)
            if (eventDto.OrganizerId.HasValue)
            {
                var organizer = await _context.Users.FindAsync(eventDto.OrganizerId);
                if (organizer == null)
                {
                    return BadRequest("Invalid OrganizerId.");
                }
            }

            var newEvent = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                EventDate = eventDto.EventDate,
                Location = eventDto.Location,
                Image = eventDto.Image,
                CreatedAt = DateTime.UtcNow,
                OrganizerId = eventDto.OrganizerId // Set the OrganizerId from the DTO
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            eventDto.Id = newEvent.Id;
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, eventDto);
        }

        // PUT api/event/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDto eventDto)
        {
            var eventEntity = await _context.Events.Include(e => e.Organizer).FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            // Check if the organizer exists in the database (if OrganizerId is provided)
            if (eventDto.OrganizerId.HasValue)
            {
                var organizer = await _context.Users.FindAsync(eventDto.OrganizerId);
                if (organizer == null)
                {
                    return BadRequest("Invalid OrganizerId.");
                }
            }

            // Update the event's details
            eventEntity.Title = eventDto.Title;
            eventEntity.Description = eventDto.Description;
            eventEntity.EventDate = eventDto.EventDate;
            eventEntity.Location = eventDto.Location;
            eventEntity.Image = eventDto.Image;
            eventEntity.OrganizerId = eventDto.OrganizerId; // Update OrganizerId

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Force reload the event data from the database, including the Organizer
            await _context.Entry(eventEntity).Reference(e => e.Organizer).LoadAsync();

            // Debugging: Log the OrganizerId and Organizer Name after update
            Console.WriteLine($"Updated Event {eventEntity.Id} with OrganizerId: {eventEntity.OrganizerId}");

            // Clear Cache-Control headers after PUT to ensure the latest data is returned
            Response.Headers.Remove("Cache-Control");
            Response.Headers.Remove("Pragma");
            Response.Headers.Remove("Expires");

            return NoContent();
        }

        // DELETE api/event/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventEntity = await _context.Events.FindAsync(id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
