using Microsoft.AspNetCore.Mvc;
using EventPlanningCapstoneProject.Models;
using Microsoft.EntityFrameworkCore;
using EventPlanningCapstoneProject.Data;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Analytics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalyticsDto>>> GetAnalytics()
        {
            return await _context.Analytics
                .Select(a => new AnalyticsDto
                {
                    Id = a.Id,
                    EventId = a.EventId,
                    SalesData = a.SalesData,
                    Demographics = a.Demographics
                }).ToListAsync();
        }

        // GET: api/Analytics/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AnalyticsDto>> GetAnalytics(int id)
        {
            var analytics = await _context.Analytics
                .Where(a => a.Id == id)
                .Select(a => new AnalyticsDto
                {
                    Id = a.Id,
                    EventId = a.EventId,
                    SalesData = a.SalesData,
                    Demographics = a.Demographics
                }).FirstOrDefaultAsync();

            if (analytics == null)
            {
                return NotFound();
            }

            return analytics;
        }

        // POST: api/Analytics
        [HttpPost]
        public async Task<ActionResult<AnalyticsDto>> PostAnalytics(AnalyticsDto analyticsDto)
        {
            var analytics = new Analytics
            {
                EventId = analyticsDto.EventId,
                SalesData = analyticsDto.SalesData,
                Demographics = analyticsDto.Demographics
            };

            _context.Analytics.Add(analytics);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnalytics), new { id = analytics.Id }, new AnalyticsDto
            {
                Id = analytics.Id,
                EventId = analytics.EventId,
                SalesData = analytics.SalesData,
                Demographics = analytics.Demographics
            });
        }

        // PUT: api/Analytics/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnalytics(int id, AnalyticsDto analyticsDto)
        {
            var analytics = await _context.Analytics.FindAsync(id);
            if (analytics == null)
            {
                return NotFound();
            }

            analytics.SalesData = analyticsDto.SalesData;
            analytics.Demographics = analyticsDto.Demographics;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Analytics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalytics(int id)
        {
            var analytics = await _context.Analytics.FindAsync(id);
            if (analytics == null)
            {
                return NotFound();
            }

            _context.Analytics.Remove(analytics);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
