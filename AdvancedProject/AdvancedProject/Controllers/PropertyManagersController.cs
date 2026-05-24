using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using AdvancedProject.ViewModels;

namespace AdvancedProject.Controllers
{
    [Authorize(Roles = "PropertyManager")]
    public class PropertyManagersController : Controller
    {
        private readonly APContext _context;

        public PropertyManagersController(APContext context)
        {
            _context = context;
        }

        // GET: PropertyManagers
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.PropertyManagers.Include(p => p.User);
            return View(await aPContext.ToListAsync());
        }

        // GET: PropertyManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyManager = await _context.PropertyManagers
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (propertyManager == null)
            {
                return NotFound();
            }

            return View(propertyManager);
        }

        // GET: PropertyManagers/MyProfile
        public async Task<IActionResult> MyProfile()
        {
            var currentUserEmail = User.Identity!.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser == null) return NotFound();

            var manager = await _context.PropertyManagers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == currentUser.UserId);
            if (manager == null) return NotFound();

            return View(manager);
        }

        // GET: PropertyManagers/EditMyProfile
        public async Task<IActionResult> EditMyProfile()
        {
            var currentUserEmail = User.Identity!.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser == null) return NotFound();

            var manager = await _context.PropertyManagers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == currentUser.UserId);
            if (manager == null) return NotFound();

            var vm = new ManagerSelfEditVM
            {
                ManagerId = manager.ManagerId,
                UserId = manager.UserId,
                Username = manager.User.Username,
                FullName = manager.User.FullName,
                Email = manager.User.Email,
                Phone = manager.User.Phone,
                Gender = manager.User.Gender
            };

            return View(vm);
        }

        // POST: PropertyManagers/EditMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyProfile(ManagerSelfEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _context.Users.FindAsync(vm.UserId);
            if (user == null) return NotFound();

            var username = vm.Username.Trim().ToLower();
            var email = vm.Email.Trim().ToLower();
            var phone = vm.Phone.Trim();

            if (_context.Users.Any(u => u.Username.ToLower() == username && u.UserId != vm.UserId))
                ModelState.AddModelError("Username", "Username already exists");
            if (_context.Users.Any(u => u.Email.ToLower() == email && u.UserId != vm.UserId))
                ModelState.AddModelError("Email", "Email already exists");
            if (_context.Users.Any(u => u.Phone == phone && u.UserId != vm.UserId))
                ModelState.AddModelError("Phone", "Phone already exists");

            if (!ModelState.IsValid) return View(vm);

            user.Username = username;
            user.FullName = vm.FullName;
            user.Email = email;
            user.Phone = phone;
            user.Gender = vm.Gender;

            if (!string.IsNullOrWhiteSpace(vm.Password))
                user.Password = vm.Password;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(MyProfile));
        }

        // GET: PropertyManagers/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: PropertyManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerId,UserId,HireDate")] PropertyManager propertyManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(propertyManager);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Property Manager was created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", propertyManager.UserId);
            return View(propertyManager);
        }

        // GET: PropertyManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyManager = await _context.PropertyManagers.FindAsync(id);
            if (propertyManager == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", propertyManager.UserId);
            return View(propertyManager);
        }

        // POST: PropertyManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,UserId,HireDate")] PropertyManager propertyManager)
        {
            if (id != propertyManager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyManagerExists(propertyManager.ManagerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Property Manager was edited successfully.";
                return RedirectToAction(nameof(Details), new { id = propertyManager.ManagerId });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", propertyManager.UserId);
            return View(propertyManager);
        }

        // GET: PropertyManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyManager = await _context.PropertyManagers
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (propertyManager == null)
            {
                return NotFound();
            }

            return View(propertyManager);
        }

        // POST: PropertyManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var propertyManager = await _context.PropertyManagers.FindAsync(id);
            if (propertyManager != null)
            {
                _context.PropertyManagers.Remove(propertyManager);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Property Manager was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyManagerExists(int id)
        {
            return _context.PropertyManagers.Any(e => e.ManagerId == id);
        }
    }
}
