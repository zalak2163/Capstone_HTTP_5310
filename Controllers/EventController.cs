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
                                        .Select(e => new EventDto
                                        {
                                            Id = e.Id,
                                            Title = e.Title,
                                            Description = e.Description,
                                            EventDate = e.EventDate,
                                            Location = e.Location,
                                            Image = e.Image,
                                            CreatedAt = e.CreatedAt,
                                            OrganizerName = e.Organizer != null ? e.Organizer.Name : "Unknown"
                                        })
                                        .ToListAsync();

            return Ok(events);
        }

        // GET api/event/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventEntity = await _context.Events
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
                                                OrganizerName = e.Organizer != null ? e.Organizer.Name : "Unknown"
                                            })
                                            .FirstOrDefaultAsync();

            if (eventEntity == null)
            {
                return NotFound();
            }

            return Ok(eventEntity);
        }

        // POST api/event
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            var newEvent = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                EventDate = eventDto.EventDate,
                Location = eventDto.Location,
                Image = eventDto.Image,
                CreatedAt = DateTime.UtcNow,
                OrganizerId = eventDto.OrganizerId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            eventDto.Id = newEvent.Id;
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, eventDto);
        }

        // PUT api/event/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDto eventDto)
        {
            var eventEntity = await _context.Events.FindAsync(id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            eventEntity.Title = eventDto.Title;
            eventEntity.Description = eventDto.Description;
            eventEntity.EventDate = eventDto.EventDate;
            eventEntity.Location = eventDto.Location;
            eventEntity.Image = eventDto.Image;
            eventEntity.OrganizerId = eventDto.OrganizerId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/event/5
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
