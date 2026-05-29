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

        try
        {
            var response = await _api.LoginAsync(model.Email, model.Password);

            if (response == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (!response.Roles.Contains("PropertyManager"))
            {
                ModelState.AddModelError(string.Empty,
                    "Access denied. This portal is restricted to Property Managers.");
                return View(model);
            }

            HttpContext.Session.SetString("JwtToken",     response.Token);
            HttpContext.Session.SetString("ManagerName",  response.FullName);
            HttpContext.Session.SetString("ManagerEmail", response.Email);

            return RedirectToAction("Index", "Reports");
        }
        catch (ApiUnavailableException)
        {
            ModelState.AddModelError(string.Empty,
                "Cannot reach the API server. Please make sure the API project is running and try again.");
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
