using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProject.Data;
using AdvancedProject.Models;

namespace AdvancedProject.Controllers
{
    public class TenantsController : Controller
    {
        private readonly APContext _context;

        public TenantsController(APContext context)
        {
            _context = context;
        }

        // GET: Tenants
        public async Task<IActionResult> Index(string searchTerm)
        {
            var tenantsQuery = _context.Users
                .Where(u => u.Role == "Tenant")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                tenantsQuery = tenantsQuery.Where(u =>
                    u.Username.Contains(searchTerm) ||
                    u.FullName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.Phone.Contains(searchTerm) ||
                    u.Role.Contains(searchTerm));
            }

            ViewData["CurrentSearchTerm"] = searchTerm;

            return View(await tenantsQuery.ToListAsync());
        }



        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            return View(new TenantCreateVM());
        }



        // POST: Tenants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TenantCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                // Age validation
                var age = DateTime.Today.Year - vm.Dob.Year;
                if (vm.Dob.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 21)
                {
                    ModelState.AddModelError("Dob", "Tenant must be at least 21 years old.");
                    return View(vm);
                }

                var username = vm.Username.Trim().ToLower();
                var email = vm.Email.Trim().ToLower();
                var phone = vm.Phone.Trim();

                // Username check
                if (_context.Users.Any(u => u.Username.ToLower() == username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                }

                // Email check
                if (_context.Users.Any(u => u.Email.ToLower() == email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }

                // Phone check
                if (_context.Users.Any(u => u.Phone == phone))
                {
                    ModelState.AddModelError("Phone", "Phone already exists");
                }

                // Stop if any duplicate found
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                // 1. Create User
                var user = new User
                {
                    Username = username,
                    Password = vm.Password,
                    FullName = vm.FullName,
                    Email = email,
                    Phone = phone,
                    Gender = vm.Gender,
                    Role = "Tenant",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 2. Create Tenant
                var tenant = new Tenant
                {
                    Dob = DateOnly.FromDateTime(vm.Dob),
                    NationalId = vm.NationalId,
                    UserId = user.UserId
                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tenant = await _context.Tenants
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TenantId == id);

            if (tenant == null) return NotFound();

            var vm = new TenantEditVM
            {
                TenantId = tenant.TenantId,
                UserId = tenant.UserId,
                Username = tenant.User.Username,
                FullName = tenant.User.FullName,
                Email = tenant.User.Email,
                Phone = tenant.User.Phone,
                Gender = tenant.User.Gender,
                Dob = tenant.Dob.ToDateTime(TimeOnly.MinValue),
                NationalId = tenant.NationalId
            };

            return View(vm);
        }

        // POST: Tenants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TenantEditVM vm)
        {
            if (ModelState.IsValid)
            {
                // Age validation
                var age = DateTime.Today.Year - vm.Dob.Year;
                if (vm.Dob > DateTime.Today.AddYears(-age)) age--;

                if (age < 21)
                {
                    ModelState.AddModelError("Dob", "Tenant must be at least 21 years old.");
                    return View(vm);
                }

                var username = vm.Username.Trim().ToLower();
                var email = vm.Email.Trim().ToLower();
                var phone = vm.Phone.Trim();

                // Username check (exclude current user)
                bool usernameExists = _context.Users
                    .Any(u => u.Username.ToLower() == username && u.UserId != vm.UserId);

                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                }

                // Email check
                bool emailExists = _context.Users
                    .Any(u => u.Email.ToLower() == email && u.UserId != vm.UserId);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }

                // Phone check
                bool phoneExists = _context.Users
                    .Any(u => u.Phone == phone && u.UserId != vm.UserId);

                if (phoneExists)
                {
                    ModelState.AddModelError("Phone", "Phone already exists");
                }

                // Stop if any validation failed
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var tenant = await _context.Tenants.FindAsync(vm.TenantId);
                var user = await _context.Users.FindAsync(vm.UserId);

                if (tenant == null || user == null)
                {
                    return NotFound();
                }

                // Update User
                user.Username = username;
                user.FullName = vm.FullName;
                user.Email = email;
                user.Phone = phone;
                user.Gender = vm.Gender;

                // optional password update
                if (!string.IsNullOrWhiteSpace(vm.Password))
                {
                    user.Password = vm.Password;
                }

                // Update Tenant
                tenant.Dob = DateOnly.FromDateTime(vm.Dob);
                tenant.NationalId = vm.NationalId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tenant = await _context.Tenants
    .FirstOrDefaultAsync(t => t.TenantId == id);

            if (tenant != null)
            {
                var user = await _context.Users.FindAsync(tenant.UserId);

                if (user != null)
                    _context.Users.Remove(user);

                _context.Tenants.Remove(tenant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TenantExists(int id)
        {
            return _context.Tenants.Any(e => e.TenantId == id);
        }
    }
}
