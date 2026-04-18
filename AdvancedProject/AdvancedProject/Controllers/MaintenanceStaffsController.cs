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
            if (!ModelState.IsValid)
            {
                ViewData["Skills"] = await _context.Skills.ToListAsync();
                return View(vm);
            }

            var username = vm.Username.Trim().ToLower();
            var email = vm.Email.Trim().ToLower();
            var phone = vm.Phone.Trim();

            // Validation for UNIQUE fields
            if (_context.Users.Any(u => u.Username.ToLower() == username))
                ModelState.AddModelError("Username", "Username already exists");

            if (_context.Users.Any(u => u.Email.ToLower() == email))
                ModelState.AddModelError("Email", "Email already exists");

            if (_context.Users.Any(u => u.Phone == phone))
                ModelState.AddModelError("Phone", "Phone already exists");

            // Stop if validation failed
            if (!ModelState.IsValid)
            {
                ViewData["Skills"] = await _context.Skills.ToListAsync();
                return View(vm);
            }

            // Create User
            var user = new User
            {
                Username = username,
                Password = vm.Password,
                FullName = vm.FullName,
                Email = email,
                Phone = phone,
                Gender = vm.Gender,
                Role = "Staff",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // may fail if duplicate slipped through
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Username, Email, or Phone already exists.");
                ViewData["Skills"] = await _context.Skills.ToListAsync();
                return View(vm);
            }

            // Create Maintenance Staff
            var staff = new MaintenanceStaff
            {
                UserId = user.UserId,
                AvailabilityStatus = "Available"
            };

            // Add Skills
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




        // GET: MaintenanceStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var staff = await _context.MaintenanceStaffs
                .Include(s => s.User)
                .Include(s => s.Skills)
                .FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null) return NotFound();

            var vm = new MaintenanceStaffEditVM
            {
                StaffId = staff.StaffId,
                UserId = staff.UserId,

                Username = staff.User.Username,
                FullName = staff.User.FullName,
                Email = staff.User.Email,
                Phone = staff.User.Phone,
                Gender = staff.User.Gender,
                Password = staff.User.Password,

                AvailabilityStatus = staff.AvailabilityStatus,
                SelectedSkillIds = staff.Skills.Select(s => s.SkillId).ToList()
            };

            ViewData["Skills"] = await _context.Skills.ToListAsync();

            return View(vm);
        }


        // POST: MaintenanceStaffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceStaffEditVM vm)
        {
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
            {
                ViewData["Skills"] = await _context.Skills.ToListAsync();
                return View(vm);
            }

            var user = await _context.Users.FindAsync(vm.UserId);
            var staff = await _context.MaintenanceStaffs
                .Include(s => s.Skills)
                .FirstOrDefaultAsync(s => s.StaffId == vm.StaffId);

            if (user == null || staff == null)
                return NotFound();

            var username = vm.Username.Trim().ToLower();
            var email = vm.Email.Trim().ToLower();
            var phone = vm.Phone.Trim();

            // UNIQUE CHECKS (IGNORE CURRENT USER)
            if (_context.Users.Any(u => u.Username.ToLower() == username && u.UserId != vm.UserId))
                ModelState.AddModelError("Username", "Username already exists");

            if (_context.Users.Any(u => u.Email.ToLower() == email && u.UserId != vm.UserId))
                ModelState.AddModelError("Email", "Email already exists");

            if (_context.Users.Any(u => u.Phone == phone && u.UserId != vm.UserId))
                ModelState.AddModelError("Phone", "Phone already exists");

            if (!ModelState.IsValid)
            {
                ViewData["Skills"] = await _context.Skills.ToListAsync();
                return View(vm);
            }

            // UPDATE USER TABLE
            user.Username = username;
            user.FullName = vm.FullName;
            user.Email = email;
            user.Phone = phone;
            user.Gender = vm.Gender;

            if (!string.IsNullOrWhiteSpace(vm.Password))
                user.Password = vm.Password;

            // UPDATE STAFF TABLE
            staff.AvailabilityStatus = vm.AvailabilityStatus;

            // UPDATE SKILLS (replace all)
            staff.Skills.Clear();

            if (vm.SelectedSkillIds != null && vm.SelectedSkillIds.Any())
            {
                var skills = await _context.Skills
                    .Where(s => vm.SelectedSkillIds.Contains(s.SkillId))
                    .ToListAsync();

                staff.Skills = skills;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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