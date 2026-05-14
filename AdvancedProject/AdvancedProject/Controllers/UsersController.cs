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
    public class UsersController : Controller
    {
        private readonly APContext _context;

        public UsersController(APContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string roleFilter, string searchTerm)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                users = users.Where(u => u.Role == roleFilter);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                users = users.Where(u =>
                    u.Username.Contains(searchTerm) ||
                    u.FullName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.Phone.Contains(searchTerm) ||
                    u.Role.Contains(searchTerm));
            }

            ViewData["CurrentRoleFilter"] = roleFilter;
            ViewData["CurrentSearchTerm"] = searchTerm;

            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Role == "Tenant")
            {
                var tenant = await _context.Tenants
                    .FirstOrDefaultAsync(t => t.UserId == user.UserId);

                if (tenant != null)
                {
                    return RedirectToAction("Details", "Tenants", new { id = tenant.TenantId });
                }
            }

            if (user.Role == "Staff")
            {
                var staff = await _context.MaintenanceStaffs
                    .FirstOrDefaultAsync(s => s.UserId == user.UserId);

                if (staff != null)
                {
                    return RedirectToAction("Details", "MaintenanceStaffs", new { id = staff.StaffId });
                }
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,FullName,Email,Phone,Role,CreatedAt")] User user)
        {
            if (ModelState.IsValid)
            {
                user.IsActive = true;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new UserEditVM
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender
                // password intentionally empty
            };

            return View(vm);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM vm)
        {
            var user = await _context.Users.FindAsync(vm.UserId);

            if (user == null)
                return NotFound();

            var username = vm.Username.Trim().ToLower();
            var email = vm.Email.Trim().ToLower();
            var phone = vm.Phone.Trim();

            // UNIQUE CHECKS (ignore current user)
            if (_context.Users.Any(u => u.Username.ToLower() == username && u.UserId != vm.UserId))
                ModelState.AddModelError("Username", "Username already exists");

            if (_context.Users.Any(u => u.Email.ToLower() == email && u.UserId != vm.UserId))
                ModelState.AddModelError("Email", "Email already exists");

            if (_context.Users.Any(u => u.Phone == phone && u.UserId != vm.UserId))
                ModelState.AddModelError("Phone", "Phone already exists");

            if (!ModelState.IsValid)
                return View(vm);

            // Update fields
            user.Username = username;
            user.FullName = vm.FullName;
            user.Email = email;
            user.Phone = phone;
            user.Gender = vm.Gender;

            // OPTIONAL password
            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                user.Password = vm.Password;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
