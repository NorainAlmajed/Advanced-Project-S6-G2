using AdvancedProjectAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        // _context will be added after APContext is ready

        // GET: api/maintenance
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("Maintenance endpoint is working");
        }

        // GET: api/maintenance/{ticketNumber}/{phone}
        [HttpGet("{ticketNumber}/{phone}")]
        public IActionResult GetByTicketAndPhone(int ticketNumber, string phone)
        {
            return Ok("Lookup endpoint is working");
        }
    }
}