using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialShareController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SocialShareController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SocialShare
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SocialShareDto>>> GetSocialShares()
        {
            var socialShares = await _context.SocialShares
                .Include(e => e.Event)  // Include related Event data
                .Include(u => u.User)   // Include related User data
                .Select(s => new SocialShareDto
                {
                    Id = s.Id,
                    EventId = s.EventId,
                    Platform = s.Platform,
                    UserId = s.UserId,
                    ShareDate = s.ShareDate
                })
                .ToListAsync();

            return Ok(socialShares);
        }

        // GET: api/SocialShare/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SocialShareDto>> GetSocialShare(int id)
        {
            var socialShare = await _context.SocialShares
                .Where(s => s.Id == id)
                .Select(s => new SocialShareDto
                {
                    Id = s.Id,
                    EventId = s.EventId,
                    Platform = s.Platform,
                    UserId = s.UserId,
                    ShareDate = s.ShareDate
                })
                .FirstOrDefaultAsync();

            if (socialShare == null)
            {
                return NotFound();
            }

            return Ok(socialShare);
        }

        // POST: api/SocialShare
        [HttpPost]
        public async Task<ActionResult<SocialShareDto>> PostSocialShare(SocialShareDto socialShareDto)
        {
            var socialShare = new SocialShare
            {
                EventId = socialShareDto.EventId,
                Platform = socialShareDto.Platform,
                UserId = socialShareDto.UserId,
                ShareDate = DateTime.Now
            };

            _context.SocialShares.Add(socialShare);
            await _context.SaveChangesAsync();

            socialShareDto.Id = socialShare.Id;

            return CreatedAtAction("GetSocialShare", new { id = socialShare.Id }, socialShareDto);
        }

        // DELETE: api/SocialShare/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocialShare(int id)
        {
            var socialShare = await _context.SocialShares.FindAsync(id);
            if (socialShare == null)
            {
                return NotFound();
            }

            _context.SocialShares.Remove(socialShare);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
