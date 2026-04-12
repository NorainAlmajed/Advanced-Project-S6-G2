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
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.MaintenanceStaffs.Include(m => m.User);
            return View(await aPContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (maintenanceStaff == null)
            {
                return NotFound();
            }

            return View(maintenanceStaff);
        }

        // GET: MaintenanceStaffs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: MaintenanceStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,AvailabilityStatus,CreatedAt,UserId")] MaintenanceStaff maintenanceStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", maintenanceStaff.UserId);
            return View(maintenanceStaff);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,AvailabilityStatus,CreatedAt,UserId")] MaintenanceStaff maintenanceStaff)
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
