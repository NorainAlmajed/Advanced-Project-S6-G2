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
    }
}