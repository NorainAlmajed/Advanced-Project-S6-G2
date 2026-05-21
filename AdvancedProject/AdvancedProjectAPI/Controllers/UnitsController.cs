using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
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

        // POST: api/units
        [HttpPost]
        [Authorize(Roles = "PropertyManager")]
        public async Task<ActionResult<Unit>> Create(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = unit.UnitId }, unit);
        }

        // PUT: api/units/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Update(int id, Unit updated)
        {
            var existing = await _context.Units.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Unit not found." });

            existing.UnitNumber = updated.UnitNumber;
            existing.UnitTypeId = updated.UnitTypeId;
            existing.SizeSqFt = updated.SizeSqFt;
            existing.RentAmount = updated.RentAmount;
            existing.AvailabilityStatus = updated.AvailabilityStatus;
            existing.IsActive = updated.IsActive;
            existing.PropertyId = updated.PropertyId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/units/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound(new { message = "Unit not found." });

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}