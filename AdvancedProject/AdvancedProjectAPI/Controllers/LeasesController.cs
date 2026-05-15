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
    }
}