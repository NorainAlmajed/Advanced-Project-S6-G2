using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitsController : ControllerBase
    {
        private readonly APContext _context;

        public UnitsController(APContext context)
        {
            _context = context;
        }

        // GET: api/units
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetAll()
        {
            var units = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .ToListAsync();

            return Ok(units);
        }

        // GET: api/units/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetById(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound(new { message = "Unit not found." });

            return Ok(unit);
        }
    }
}