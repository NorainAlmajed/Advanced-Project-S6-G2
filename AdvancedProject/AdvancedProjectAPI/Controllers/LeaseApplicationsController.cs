using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaseApplicationsController : ControllerBase
    {
        private readonly APContext _context;

        public LeaseApplicationsController(APContext context)
        {
            _context = context;
        }

        // GET: api/leaseapplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaseApplication>>> GetAll()
        {
            var applications = await _context.LeaseApplications
                .Include(a => a.Tenant).ThenInclude(t => t.User)
                .Include(a => a.Unit).ThenInclude(u => u.Property)
                .ToListAsync();
            return Ok(applications);
        }

        // GET: api/leaseapplications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaseApplication>> GetById(int id)
        {
            var application = await _context.LeaseApplications
                .Include(a => a.Tenant).ThenInclude(t => t.User)
                .Include(a => a.Unit).ThenInclude(u => u.Property)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                return NotFound(new { message = "Lease application not found." });
            return Ok(application);
        }

        // POST: api/leaseapplications
        [HttpPost]
        public async Task<ActionResult<LeaseApplication>> Create(LeaseApplication application)
        {
            _context.LeaseApplications.Add(application);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = application.ApplicationId }, application);
        }

        // PUT: api/leaseapplications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LeaseApplication updated)
        {
            var existing = await _context.LeaseApplications.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Lease application not found." });

            existing.Status = updated.Status;
            existing.StartDate = updated.StartDate;
            existing.DurationId = updated.DurationId;
            existing.ApproveTime = updated.ApproveTime;
            existing.RejectTime = updated.RejectTime;
            existing.TenantId = updated.TenantId;
            existing.UnitId = updated.UnitId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/leaseapplications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.LeaseApplications.FindAsync(id);
            if (application == null) return NotFound(new { message = "Lease application not found." });

            _context.LeaseApplications.Remove(application);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}