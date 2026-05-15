using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
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
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAll()
        {
            var tenants = await _context.Tenants
                .Include(t => t.User)
                .ToListAsync();

            return Ok(tenants);
        }

        // GET: api/tenants/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetById(int id)
        {
            var tenant = await _context.Tenants
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TenantId == id);

            if (tenant == null)
                return NotFound(new { message = "Tenant not found." });

            return Ok(tenant);
        }
    }
}