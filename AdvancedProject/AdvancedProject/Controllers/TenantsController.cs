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

                var age = DateTime.Today.Year - vm.Dob.Year;

                if (vm.Dob.Date > DateTime.Today.AddYears(-age))
                {
                    age--; // adjust if birthday hasn't happened yet this year
                }

                if (age < 21)
                {
                    ModelState.AddModelError("Dob", "Tenant must be at least 21 years old.");
                    return View(vm);
                }

                var username = vm.Username.ToLower();

                if (_context.Users.Any(u => u.Username.ToLower() == username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(vm);
                }


                // 1. Create User
                var user = new User
                {
                    Username = vm.Username.ToLower(),
                    Password = vm.Password,
                    FullName = vm.FullName,
                    Email = vm.Email,
                    Phone = vm.Phone,
                    Gender = vm.Gender,
                    Role = "Tenant",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // VERY IMPORTANT (to get UserId)

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
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tenant.UserId);
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TenantId,Dob,NationalId,UserId")] Tenant tenant)
        {
            if (id != tenant.TenantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.TenantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tenant.UserId);
            return View(tenant);
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
