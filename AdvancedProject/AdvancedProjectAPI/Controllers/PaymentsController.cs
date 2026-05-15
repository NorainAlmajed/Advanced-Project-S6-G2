using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly APContext _context;

        public PaymentsController(APContext context)
        {
            _context = context;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
        {
            var payments = await _context.Payments
                .Include(p => p.Lease).ThenInclude(l => l.Tenant).ThenInclude(t => t.User)
                .Include(p => p.PaymentMethod)
                .ToListAsync();
            return Ok(payments);
        }

        // GET: api/payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetById(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Lease).ThenInclude(l => l.Tenant).ThenInclude(t => t.User)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
                return NotFound(new { message = "Payment not found." });
            return Ok(payment);
        }

        // POST: api/payments
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
        }

        // PUT: api/payments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Payment updated)
        {
            var existing = await _context.Payments.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Payment not found." });

            existing.Amount = updated.Amount;
            existing.Status = updated.Status;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.PaymentMethodId = updated.PaymentMethodId;
            existing.PaymentFrequencyId = updated.PaymentFrequencyId;
            existing.LeaseId = updated.LeaseId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/payments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound(new { message = "Payment not found." });

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}