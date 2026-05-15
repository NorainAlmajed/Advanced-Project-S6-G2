using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeasesController : ControllerBase
    {
        private readonly APContext _context;

        public LeasesController(APContext context)
        {
            _context = context;
        }

        // GET: api/leases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lease>>> GetAll()
        {
            var leases = await _context.Leases
                .Include(l => l.Tenant).ThenInclude(t => t.User)
                .Include(l => l.Unit).ThenInclude(u => u.Property)
                .ToListAsync();
            return Ok(leases);
        }

        // GET: api/leases/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Lease>> GetById(int id)
        {
            var lease = await _context.Leases
                .Include(l => l.Tenant).ThenInclude(t => t.User)
                .Include(l => l.Unit).ThenInclude(u => u.Property)
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (lease == null)
                return NotFound(new { message = "Lease not found." });
            return Ok(lease);
        }

        // POST: api/leases
        [HttpPost]
        public async Task<ActionResult<Lease>> Create(Lease lease)
        {
            _context.Leases.Add(lease);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = lease.LeaseId }, lease);
        }

        // PUT: api/leases/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Lease updated)
        {
            var existing = await _context.Leases.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Lease not found." });

            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.MonthlyRent = updated.MonthlyRent;
            existing.Status = updated.Status;
            existing.TenantId = updated.TenantId;
            existing.UnitId = updated.UnitId;
            existing.DurationId = updated.DurationId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/leases/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease == null) return NotFound(new { message = "Lease not found." });

            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}