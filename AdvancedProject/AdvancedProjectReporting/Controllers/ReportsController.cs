using AdvancedProjectReporting.Dtos;
using AdvancedProjectReporting.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedProjectReporting.Controllers;

public class ReportsController : Controller
{
    private readonly ApiClient _api;

    public ReportsController(ApiClient api)
    {
        _api = api;
    }

    // GET /Reports  →  Dashboard
    public async Task<IActionResult> Index()
    {
        var occupancy    = await _api.GetOccupancyReportAsync();
        var maintenance  = await _api.GetMaintenanceReportAsync();
        var payments     = await _api.GetPaymentReportAsync();

        var model = new DashboardViewModel
        {
            Occupancy   = occupancy,
            Maintenance = maintenance,
            Payments    = payments
        };

        return View(model);
    }

    // GET /Reports/Occupancy
    public async Task<IActionResult> Occupancy()
    {
        var data = await _api.GetOccupancyReportAsync();
        return View(data);
    }

    // GET /Reports/Maintenance
    public async Task<IActionResult> Maintenance()
    {
        var data = await _api.GetMaintenanceReportAsync();
        return View(data);
    }

    // GET /Reports/Payments
    public async Task<IActionResult> Payments()
    {
        var data = await _api.GetPaymentReportAsync();
        return View(data);
    }
}
