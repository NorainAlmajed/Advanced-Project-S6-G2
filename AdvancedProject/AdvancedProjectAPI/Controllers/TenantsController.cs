using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly APContext _context;

        public TenantsController(APContext context)
        {
            _context = context;
        }

        // GET: api/tenants
        [HttpGet]
        [Authorize(Roles = "PropertyManager")]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAll()
        {
            var tenants = await _context.Tenants
                .Include(t => t.User)
                .ToListAsync();
            return Ok(tenants);
        }

        // GET: api/tenants/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Tenant>> GetById(int id)
        {
            var tenant = await _context.Tenants
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TenantId == id);

            if (tenant == null)
                return NotFound(new { message = "Tenant not found." });
            return Ok(tenant);
        }

        // POST: api/tenants
        [HttpPost]
        [Authorize(Roles = "PropertyManager")]
        public async Task<ActionResult<Tenant>> Create(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tenant.TenantId }, tenant);
        }

        // PUT: api/tenants/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Update(int id, Tenant updated)
        {
            var existing = await _context.Tenants.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Tenant not found." });

            existing.Dob = updated.Dob;
            existing.NationalId = updated.NationalId;
            existing.Salary = updated.Salary;
            existing.MaritalStatus = updated.MaritalStatus;
            existing.EmploymentStatus = updated.EmploymentStatus;
            existing.FinancialStability = updated.FinancialStability;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tenants/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return NotFound(new { message = "Tenant not found." });

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}