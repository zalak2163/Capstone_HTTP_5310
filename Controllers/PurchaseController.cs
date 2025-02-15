using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/purchase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchases()
        {
            var purchases = await _context.Purchases
                                          .Select(p => new PurchaseDto
                                          {
                                              Id = p.Id,
                                              UserId = p.UserId,
                                              EventId = p.EventId,
                                              TicketId = p.TicketId,
                                              Quantity = p.Quantity,
                                              TotalPrice = p.TotalPrice,
                                              PaymentStatus = p.PaymentStatus,
                                              PurchaseDate = p.PurchaseDate
                                          })
                                          .ToListAsync();

            return Ok(purchases);
        }

        // GET api/purchase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(int id)
        {
            var purchase = await _context.Purchases
                                          .Where(p => p.Id == id)
                                          .Select(p => new PurchaseDto
                                          {
                                              Id = p.Id,
                                              UserId = p.UserId,
                                              EventId = p.EventId,
                                              TicketId = p.TicketId,
                                              Quantity = p.Quantity,
                                              TotalPrice = p.TotalPrice,
                                              PaymentStatus = p.PaymentStatus,
                                              PurchaseDate = p.PurchaseDate
                                          })
                                          .FirstOrDefaultAsync();

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        // POST api/purchase
        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> PostPurchase(PurchaseDto purchaseDto)
        {
            var purchase = new Purchase
            {
                UserId = purchaseDto.UserId,
                EventId = purchaseDto.EventId,
                TicketId = purchaseDto.TicketId,
                Quantity = purchaseDto.Quantity,
                TotalPrice = purchaseDto.TotalPrice,
                PaymentStatus = purchaseDto.PaymentStatus,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            purchaseDto.Id = purchase.Id;
            return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchaseDto);
        }

        // PUT api/purchase/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, PurchaseDto purchaseDto)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            purchase.UserId = purchaseDto.UserId;
            purchase.EventId = purchaseDto.EventId;
            purchase.TicketId = purchaseDto.TicketId;
            purchase.Quantity = purchaseDto.Quantity;
            purchase.TotalPrice = purchaseDto.TotalPrice;
            purchase.PaymentStatus = purchaseDto.PaymentStatus;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/purchase/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
