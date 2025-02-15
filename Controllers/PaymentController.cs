using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            var payments = await _context.Payments
                                         .Select(p => new PaymentDto
                                         {
                                             Id = p.Id,
                                             PurchaseId = p.PurchaseId,
                                             PaymentMethod = p.PaymentMethod,
                                             Amount = p.Amount,
                                             PaymentStatus = p.PaymentStatus,
                                             TransactionId = p.TransactionId,
                                             PaymentDate = p.PaymentDate
                                         })
                                         .ToListAsync();

            return Ok(payments);
        }

        // GET api/payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            var payment = await _context.Payments
                                         .Where(p => p.Id == id)
                                         .Select(p => new PaymentDto
                                         {
                                             Id = p.Id,
                                             PurchaseId = p.PurchaseId,
                                             PaymentMethod = p.PaymentMethod,
                                             Amount = p.Amount,
                                             PaymentStatus = p.PaymentStatus,
                                             TransactionId = p.TransactionId,
                                             PaymentDate = p.PaymentDate
                                         })
                                         .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // POST api/payment
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> PostPayment(PaymentDto paymentDto)
        {
            var payment = new Payment
            {
                PurchaseId = paymentDto.PurchaseId,
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = paymentDto.Amount,
                PaymentStatus = paymentDto.PaymentStatus,
                TransactionId = paymentDto.TransactionId,
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            paymentDto.Id = payment.Id;
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, paymentDto);
        }

        // PUT api/payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentDto paymentDto)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            payment.PurchaseId = paymentDto.PurchaseId;
            payment.PaymentMethod = paymentDto.PaymentMethod;
            payment.Amount = paymentDto.Amount;
            payment.PaymentStatus = paymentDto.PaymentStatus;
            payment.TransactionId = paymentDto.TransactionId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
