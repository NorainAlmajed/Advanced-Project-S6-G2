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

    // When the JWT expires the API returns 401 — clear the session and
    // send the user back to login with an informative message.
    private IActionResult HandleExpiredSession()
    {
        HttpContext.Session.Clear();
        TempData["SessionExpired"] = "Your session has expired. Please sign in again.";
        return RedirectToAction("Index", "Login");
    }

    public async Task<IActionResult> Index()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        try
        {
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
        catch (SessionExpiredException)    { return HandleExpiredSession(); }
        catch (ApiUnavailableException)    { return View("ApiError"); }
    }

    public async Task<IActionResult> Occupancy()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        try
        {
            return View(await _api.GetOccupancyReportAsync());
        }
        catch (SessionExpiredException) { return HandleExpiredSession(); }
        catch (ApiUnavailableException) { return View("ApiError"); }
    }

    public async Task<IActionResult> Maintenance()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        try
        {
            return View(await _api.GetMaintenanceReportAsync());
        }
        catch (SessionExpiredException) { return HandleExpiredSession(); }
        catch (ApiUnavailableException) { return View("ApiError"); }
    }

    public async Task<IActionResult> Payments()
    {
        var auth = RequireAuth();
        if (auth != null) return auth;

        try
        {
            return View(await _api.GetPaymentReportAsync());
        }
        catch (SessionExpiredException) { return HandleExpiredSession(); }
        catch (ApiUnavailableException) { return View("ApiError"); }
    }
}
