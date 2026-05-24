using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using AdvancedProject.ViewModels;
using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly APContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, APContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.FirstName = user?.FullName?.Split(' ')[0] ?? user?.FullName ?? "there";
                ViewBag.PropertyCount = await _context.Properties.CountAsync(p => p.IsActive);
                ViewBag.UnitCount     = await _context.Units.CountAsync(u => u.IsActive);
            }
            return View(new MaintenanceLookupVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MaintenanceLookupVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // ── Step 1: try the Web API via HttpClient (assignment requirement) ──
            try
            {
                var client = _httpClientFactory.CreateClient("MaintenanceApi");
                var phone = Uri.EscapeDataString(vm.Phone.Trim());
                var response = await client.GetAsync($"api/maintenance/{vm.TicketNumber}/{phone}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    vm.Result = JsonSerializer.Deserialize<MaintenanceResultVM>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    vm.NotFound = true;
                }

                return View(vm);
            }
            catch (HttpRequestException)
            {
                // API project is not running — fall through to direct DB lookup below
                _logger.LogWarning("Maintenance API unreachable; falling back to direct DB query.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error calling maintenance API; falling back to direct DB query.");
            }

            // ── Step 2: fallback — query the database directly (dev convenience) ──
            try
            {
                var request = await _context.MaintenanceRequests
                    .Include(m => m.Unit)
                    .Include(m => m.Skill)
                    .Include(m => m.AssignedStaff).ThenInclude(s => s!.User)
                    .Include(m => m.User)
                    .Where(m => m.RequestId == vm.TicketNumber && m.User.Phone == vm.Phone.Trim())
                    .FirstOrDefaultAsync();

                if (request != null)
                {
                    vm.Result = new MaintenanceResultVM
                    {
                        RequestId      = request.RequestId,
                        Status         = request.Status,
                        Priority       = request.Priority,
                        Notes          = request.Notes,
                        RequestDate    = request.RequestDate,
                        UnitNumber     = request.Unit?.UnitNumber ?? "—",
                        SkillName      = request.Skill?.Name ?? "—",
                        AssignedStaffName = request.AssignedStaff?.User?.FullName
                    };
                }
                else
                {
                    vm.NotFound = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fallback DB lookup.");
                ModelState.AddModelError("", "Unable to reach the lookup service. Please try again later.");
            }

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
