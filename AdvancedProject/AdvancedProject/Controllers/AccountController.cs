using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using AdvancedProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly APContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            APContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Age validation - must be at least 21
                var age = DateTime.Today.Year - model.Dob.Year;
                if (model.Dob.Date > DateTime.Today.AddYears(-age)) age--;
                if (age < 21)
                {
                    ModelState.AddModelError("Dob", "Tenant must be at least 21 years old.");
                    return View(model);
                }

                var username = model.Username.Trim().ToLower();
                var email = model.Email.Trim().ToLower();
                var phone = model.Phone.Trim();

                // Check duplicates without revealing which field conflicts
                bool duplicateFound = _context.Users.Any(u => u.Username.ToLower() == username)
                    || _context.Users.Any(u => u.Email.ToLower() == email)
                    || _context.Users.Any(u => u.Phone == phone);

                if (duplicateFound)
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please review your details and try again.");
                    return View(model);
                }

                // 1. Create Identity user
                var identityUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = model.FullName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    // 2. Assign Tenant role
                    await _userManager.AddToRoleAsync(identityUser, "Tenant");

                    // 3. Create User record in your existing table
                    var user = new User
                    {
                        Username = username,
                        Password = _passwordHasher.HashPassword(null!, model.Password),
                        FullName = model.FullName,
                        Email = email,
                        Phone = phone,
                        Gender = model.Gender,
                        Role = "Tenant",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // 4. Link Identity user to your User table
                    identityUser.UserId = user.UserId;
                    await _userManager.UpdateAsync(identityUser);

                    // 5. Create Tenant record
                    var tenant = new Tenant
                    {
                        UserId = user.UserId,
                        Dob = DateOnly.FromDateTime(model.Dob),
                        NationalId = model.NationalId,
                        Salary = model.Salary,
                        MaritalStatus = model.MaritalStatus,
                        EmploymentStatus = model.EmploymentStatus,
                        FinancialStability = "Undetermined"
                    };

                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();

                    // 6. Sign in
                    await _signInManager.SignInAsync(identityUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password,
                    model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                if (result.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "Account locked. Try again in 5 minutes.");
                else
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}