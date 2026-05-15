using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly APContext _context;

        public PropertiesController(APContext context)
        {
            _context = context;
        }

        // GET: api/properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetAll()
        {
            var properties = await _context.Properties
                .Include(p => p.Governorate)
                .Include(p => p.Units)
                .ToListAsync();
            return Ok(properties);
        }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetById(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Governorate)
                .Include(p => p.Units)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
                return NotFound(new { message = "Property not found." });
            return Ok(property);
        }

        // POST: api/properties
        [HttpPost]
        public async Task<ActionResult<Property>> Create(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = property.PropertyId }, property);
        }

        // PUT: api/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Property updated)
        {
            var existing = await _context.Properties.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Property not found." });

            existing.Name = updated.Name;
            existing.Building = updated.Building;
            existing.Road = updated.Road;
            existing.Block = updated.Block;
            existing.City = updated.City;
            existing.Description = updated.Description;
            existing.IsActive = updated.IsActive;
            existing.GovernorateId = updated.GovernorateId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound(new { message = "Property not found." });

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}