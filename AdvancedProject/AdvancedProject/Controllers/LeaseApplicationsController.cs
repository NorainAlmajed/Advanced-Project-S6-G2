using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProject.Data;
using AdvancedProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdvancedProject.Controllers
{
    public class LeaseApplicationsController : Controller
    {
        private readonly APContext _context;

        public LeaseApplicationsController(APContext context)
        {
            _context = context;
        }

        // GET: LeaseApplications
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.LeaseApplications.Include(l => l.Unit).Include(l => l.Tenant).ThenInclude(e => e.User).Include(l => l.Duration);
            return View(await aPContext.ToListAsync());
        }

        // GET: LeaseApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseApplication = await _context.LeaseApplications
                .Include(l => l.Tenant).ThenInclude(e => e.User)
                .Include(l => l.Unit).Include(l => l.Duration)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (leaseApplication == null)
            {
                return NotFound();
            }

            return View(leaseApplication);
        }

        // GET: LeaseApplications/Create
        public IActionResult Create(int? unitId)
        {
            var model = new LeaseApplication
            {
                UnitId = unitId ?? 0,
                StartDate = DateTime.Today.AddDays(1) // 👈 tomorrow
            };

            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");

            return View(model);
        }

        // POST: LeaseApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaseApplication leaseApplication)
        {
            leaseApplication.TenantId = 1;
            leaseApplication.Status = "Pending";
            leaseApplication.ApplicationDate = DateTime.Now;

            ModelState.Remove("Tenant");
            ModelState.Remove("Unit");
            ModelState.Remove("Duration");
            ModelState.Remove("Status");

            if (leaseApplication.StartDate.Date <= DateTime.Today)
            {
                ModelState.AddModelError("StartDate", "Start date must be in the future.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");
                return View(leaseApplication);
            }

            _context.Add(leaseApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaseApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseApplication = await _context.LeaseApplications.FindAsync(id);
            if (leaseApplication == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", leaseApplication.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", leaseApplication.UnitId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months", leaseApplication.DurationId);
            return View(leaseApplication);
        }

        // POST: LeaseApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,TenantId,UnitId,ApplicationDate,Status,ApproveTime,RejectTime,StartDate,DurationId")] LeaseApplication leaseApplication)
        {
            if (id != leaseApplication.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaseApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseApplicationExists(leaseApplication.ApplicationId))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", leaseApplication.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", leaseApplication.UnitId);
            return View(leaseApplication);
        }

        // GET: LeaseApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseApplication = await _context.LeaseApplications
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (leaseApplication == null)
            {
                return NotFound();
            }

            return View(leaseApplication);
        }

        // POST: LeaseApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaseApplication = await _context.LeaseApplications.FindAsync(id);
            if (leaseApplication != null)
            {
                _context.LeaseApplications.Remove(leaseApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseApplicationExists(int id)
        {
            return _context.LeaseApplications.Any(e => e.ApplicationId == id);
        }
    }
}
