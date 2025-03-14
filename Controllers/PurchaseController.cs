﻿using EventPlanningCapstoneProject.Data;
using EventPlanningCapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventPlanningCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(AppDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;  // Injecting the logger properly
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(int id)
        {
            // Log the received ID
            _logger.LogInformation($"Received request for purchase with ID: {id}");

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _context.Purchases
                                        .Include(p => p.Ticket)  // Include the associated ticket to check for dependencies
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }

            // Optional: Check if there are any other purchases for the same Ticket
            var otherPurchases = await _context.Purchases
                                                .Where(p => p.TicketId == purchase.TicketId && p.Id != id)
                                                .ToListAsync();

            if (otherPurchases.Any())
            {
                // You can either notify the user or disallow deleting this purchase because it's linked to others
                return BadRequest("This purchase is linked to other purchases for the same ticket.");
            }

            // If all checks pass, delete the purchase
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}