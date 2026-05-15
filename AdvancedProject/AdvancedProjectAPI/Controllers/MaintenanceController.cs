using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Dtos;
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
    }
}