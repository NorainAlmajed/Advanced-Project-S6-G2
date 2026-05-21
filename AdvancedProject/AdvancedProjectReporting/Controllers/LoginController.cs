using AdvancedProjectReporting.Dtos;
using AdvancedProjectReporting.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedProjectReporting.Controllers;

public class LoginController : Controller
{
    private readonly ApiClient _api;

    public LoginController(ApiClient api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            return RedirectToAction("Index", "Reports");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _api.LoginAsync(model.Email, model.Password);

        if (response == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // Verify the user is a Property Manager
        if (!response.Roles.Contains("PropertyManager"))
        {
            ModelState.AddModelError(string.Empty,
                "Access denied. This portal is restricted to Property Managers.");
            return View(model);
        }

        HttpContext.Session.SetString("JwtToken",    response.Token);
        HttpContext.Session.SetString("ManagerName", response.FullName);
        HttpContext.Session.SetString("ManagerEmail", response.Email);

        return RedirectToAction("Index", "Reports");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
