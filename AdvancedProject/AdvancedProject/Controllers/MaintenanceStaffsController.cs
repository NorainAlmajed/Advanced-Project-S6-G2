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
    public class MaintenanceStaffsController : Controller
    {
        private readonly APContext _context;

        public MaintenanceStaffsController(APContext context)
        {
            _context = context;
        }

        // GET: MaintenanceStaffs
        public async Task<IActionResult> Index(string searchTerm, string availabilityFilter, List<int> skillIds)
        {
            var staffQuery = _context.MaintenanceStaffs
                .Include(m => m.User)
                .Include(m => m.Skills)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                staffQuery = staffQuery.Where(m =>
                    m.User.Username.Contains(searchTerm) ||
                    m.User.FullName.Contains(searchTerm) ||
                    m.User.Email.Contains(searchTerm) ||
                    m.User.Phone.Contains(searchTerm) ||
                    m.User.Role.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(availabilityFilter))
            {
                staffQuery = staffQuery.Where(m => m.AvailabilityStatus == availabilityFilter);
            }

            if (skillIds != null && skillIds.Any())
            {
                foreach (var skillId in skillIds)
                {
                    staffQuery = staffQuery.Where(m => m.Skills.Any(s => s.SkillId == skillId));
                }
            }

            var staffList = await staffQuery.ToListAsync();
            var allSkills = await _context.Skills.ToListAsync();

            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentAvailabilityFilter"] = availabilityFilter;
            ViewData["CurrentSkillIds"] = skillIds ?? new List<int>();
            ViewData["Skills"] = allSkills;

            if (skillIds != null && skillIds.Count > 1 && !staffList.Any())
            {
                ViewData["SkillMatchMessage"] = "No maintenance staff member has all the selected skills together.";
            }

            return View(staffList);
        }

        // GET: MaintenanceStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs
                .Include(m => m.User)
                .Include(m => m.Skills)
                .FirstOrDefaultAsync(m => m.StaffId == id);

            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Skills"] = await _context.Skills.ToListAsync();
            return View(new MaintenanceStaffCreateVM());
        }

        // POST: MaintenanceStaffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceStaffCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var username = vm.Username.Trim().ToLower();

                // Username check
                if (_context.Users.Any(u => u.Username.ToLower() == username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    ViewData["Skills"] = await _context.Skills.ToListAsync();
                    return View(vm);
                }

                // 1. Create User
                var user = new User
                {
                    Username = username,
                    Password = vm.Password,
                    FullName = vm.FullName,
                    Email = vm.Email,
                    Phone = vm.Phone,
                    Gender = vm.Gender,
                    Role = "MaintenanceStaff", // ⚠️ not Tenant
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 2. Create Staff
                var staff = new MaintenanceStaff
                {
                    UserId = user.UserId,
                    AvailabilityStatus = "Available" // or whatever value you use
                };

                // 3. Add Skills (optional)
                if (vm.SelectedSkillIds != null && vm.SelectedSkillIds.Any())
                {
                    var skills = await _context.Skills
                        .Where(s => vm.SelectedSkillIds.Contains(s.SkillId))
                        .ToListAsync();

                    staff.Skills = skills;
                }

                _context.MaintenanceStaffs.Add(staff);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Skills"] = await _context.Skills.ToListAsync();
            return View(vm);
        }

        // GET: MaintenanceStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs.FindAsync(id);
            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", maintenanceStaff.UserId);
            return View(maintenanceStaff);
        }

        // POST: MaintenanceStaffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,AvailabilityStatus,UserId")] MaintenanceStaff maintenanceStaff)
        {
            if (id != maintenanceStaff.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceStaffExists(maintenanceStaff.StaffId))
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

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", maintenanceStaff.UserId);
            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceStaff = await _context.MaintenanceStaffs
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.StaffId == id);

            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            return View(maintenanceStaff);
        }

        // POST: MaintenanceStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceStaff = await _context.MaintenanceStaffs.FindAsync(id);
            if (maintenanceStaff != null)
            {
                _context.MaintenanceStaffs.Remove(maintenanceStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceStaffExists(int id)
        {
            return _context.MaintenanceStaffs.Any(e => e.StaffId == id);
        }
    }
}