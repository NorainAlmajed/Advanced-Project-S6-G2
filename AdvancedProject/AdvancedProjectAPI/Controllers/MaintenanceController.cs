using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Dtos;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly APContext _context;

        public MaintenanceController(APContext context)
        {
            _context = context;
        }

        // GET: api/maintenance
        [HttpGet]
        [Authorize(Roles = "PropertyManager,MaintenanceStaff")]
        public async Task<ActionResult<IEnumerable<MaintenanceRequestDto>>> GetAll()
        {
            var requests = await _context.MaintenanceRequests
                .Include(m => m.Unit)
                .Include(m => m.Skill)
                .Include(m => m.AssignedStaff).ThenInclude(s => s.User)
                .Select(m => new MaintenanceRequestDto
                {
                    RequestId = m.RequestId,
                    Status = m.Status,
                    Priority = m.Priority,
                    Notes = m.Notes,
                    RequestDate = m.RequestDate,
                    UnitNumber = m.Unit.UnitNumber,
                    SkillName = m.Skill.Name,
                    AssignedStaffName = m.AssignedStaff != null ? m.AssignedStaff.User.FullName : null
                })
                .ToListAsync();

            return Ok(requests);
        }

        // GET: api/maintenance/{ticketNumber}/{phone}
        [HttpGet("{ticketNumber}/{phone}")]
        [AllowAnonymous]
        public async Task<ActionResult<MaintenanceRequestDto>> GetByTicketAndPhone(int ticketNumber, string phone)
        {
            var request = await _context.MaintenanceRequests
                .Include(m => m.Unit)
                .Include(m => m.Skill)
                .Include(m => m.AssignedStaff).ThenInclude(s => s.User)
                .Include(m => m.User)
                .Where(m => m.RequestId == ticketNumber && m.User.Phone == phone)
                .Select(m => new MaintenanceRequestDto
                {
                    RequestId = m.RequestId,
                    Status = m.Status,
                    Priority = m.Priority,
                    Notes = m.Notes,
                    RequestDate = m.RequestDate,
                    UnitNumber = m.Unit.UnitNumber,
                    SkillName = m.Skill.Name,
                    AssignedStaffName = m.AssignedStaff != null ? m.AssignedStaff.User.FullName : null
                })
                .FirstOrDefaultAsync();

            if (request == null)
                return NotFound(new { message = "No maintenance request found with this ticket number and phone." });

            return Ok(request);
        }

        // GET: api/maintenance/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MaintenanceRequest>> GetById(int id)
        {
            var request = await _context.MaintenanceRequests
                .Include(m => m.Unit)
                .Include(m => m.Skill)
                .Include(m => m.AssignedStaff).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (request == null)
                return NotFound(new { message = "Maintenance request not found." });

            return Ok(request);
        }

        // POST: api/maintenance
        [HttpPost]
        [Authorize(Roles = "Tenant,PropertyManager")]
        public async Task<ActionResult<MaintenanceRequest>> Create(MaintenanceRequest request)
        {
            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = request.RequestId }, request);
        }

        // PUT: api/maintenance/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "PropertyManager,MaintenanceStaff")]
        public async Task<IActionResult> Update(int id, MaintenanceRequest updated)
        {
            var existing = await _context.MaintenanceRequests.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Maintenance request not found." });

            existing.Status = updated.Status;
            existing.Priority = updated.Priority;
            existing.Notes = updated.Notes;
            existing.AssignedStaffId = updated.AssignedStaffId;
            existing.SkillId = updated.SkillId;
            existing.UnitId = updated.UnitId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/maintenance/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound(new { message = "Maintenance request not found." });

            _context.MaintenanceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}