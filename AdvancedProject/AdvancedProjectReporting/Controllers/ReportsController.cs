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

    private IActionResult? RequireAuth()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            return RedirectToAction("Index", "Login");
        return null;
    }

    public async Task<IActionResult> Index()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        var occupancy   = await _api.GetOccupancyReportAsync();
        var maintenance = await _api.GetMaintenanceReportAsync();
        var payments    = await _api.GetPaymentReportAsync();

        return View(new DashboardViewModel
        {
            Occupancy   = occupancy,
            Maintenance = maintenance,
            Payments    = payments
        });
    }

    public async Task<IActionResult> Occupancy()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        return View(await _api.GetOccupancyReportAsync());
    }

    public async Task<IActionResult> Maintenance()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        return View(await _api.GetMaintenanceReportAsync());
    }

    public async Task<IActionResult> Payments()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        return View(await _api.GetPaymentReportAsync());
    }
}
