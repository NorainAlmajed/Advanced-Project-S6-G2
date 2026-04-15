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
    public class MaintenanceRequestsController : Controller
    {
        private readonly APContext _context;

        public MaintenanceRequestsController(APContext context)
        {
            _context = context;
        }

        // GET: MaintenanceRequests
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.MaintenanceRequests.Include(m => m.AssignedStaff).Include(m => m.Skill).Include(m => m.Tenant).Include(m => m.Unit);
            return View(await aPContext.ToListAsync());
        }

        // GET: MaintenanceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _context.MaintenanceRequests
                .Include(m => m.AssignedStaff)
                .Include(m => m.Skill)
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Create
        public IActionResult Create()
        {
            int tenantId = 1; // temporary (later replace with logged-in user)

            var units = _context.Leases
                .Include(l => l.Unit)
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .Select(l => l.Unit)
                .Distinct()
                .ToList();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "UnitNumber");

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name");

            return View();
        }

        // POST: MaintenanceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,SkillId,Priority,Notes")] MaintenanceRequest maintenanceRequest)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                maintenanceRequest.TenantId = 1;
                maintenanceRequest.RequestDate = DateTime.Now;
                maintenanceRequest.Status = "Pending";
                maintenanceRequest.AssignedStaffId = null;

                _context.Add(maintenanceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            int tenantId = 1;

            var units = _context.Leases
                .Include(l => l.Unit)
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .Select(l => l.Unit)
                .Distinct()
                .ToList();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "UnitNumber", maintenanceRequest.UnitId);

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", maintenanceRequest.SkillId);

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }
            ViewData["AssignedStaffId"] = new SelectList(_context.MaintenanceStaffs, "StaffId", "StaffId", maintenanceRequest.AssignedStaffId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillId", maintenanceRequest.SkillId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", maintenanceRequest.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", maintenanceRequest.UnitId);
            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,UnitId,TenantId,RequestDate,SkillId,Priority,Status,AssignedStaffId,Notes,CompletedDate,AssignedTime,ResolvedTime,ClosedTime,InProgressTime")] MaintenanceRequest maintenanceRequest)
        {
            if (id != maintenanceRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceRequestExists(maintenanceRequest.RequestId))
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
            ViewData["AssignedStaffId"] = new SelectList(_context.MaintenanceStaffs, "StaffId", "StaffId", maintenanceRequest.AssignedStaffId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillId", maintenanceRequest.SkillId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", maintenanceRequest.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", maintenanceRequest.UnitId);
            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRequest = await _context.MaintenanceRequests
                .Include(m => m.AssignedStaff)
                .Include(m => m.Skill)
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceRequestExists(int id)
        {
            return _context.MaintenanceRequests.Any(e => e.RequestId == id);
        }
    }
}
