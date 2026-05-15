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
    }
}