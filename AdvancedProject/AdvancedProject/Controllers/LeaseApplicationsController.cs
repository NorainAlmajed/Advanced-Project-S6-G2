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
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, string dateFilter)
        {
            var applicationsQuery = _context.LeaseApplications
     .Include(l => l.Tenant)
         .ThenInclude(t => t.User)
     .Include(l => l.Unit)
         .ThenInclude(u => u.Property)
     .Include(l => l.Duration)
     .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                applicationsQuery = applicationsQuery.Where(l =>
                    l.ApplicationId.ToString().Contains(searchTerm) ||
                    l.Tenant.User.FullName.Contains(searchTerm) ||
                    l.Unit.UnitNumber.Contains(searchTerm) ||
                    l.Unit.Property.Name.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                applicationsQuery = applicationsQuery.Where(l => l.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(dateFilter))
            {
                if (dateFilter == "Latest")
                {
                    applicationsQuery = applicationsQuery
                        .OrderByDescending(l => l.ApplicationDate);
                }
                else
                {
                    applicationsQuery = applicationsQuery
                        .OrderBy(l => l.ApplicationDate);
                }
            }
            else
            {
                applicationsQuery = applicationsQuery
                    .OrderByDescending(l => l.ApplicationDate);
            }

            var applications = await applicationsQuery.ToListAsync();

            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentDateFilter"] = dateFilter;
            ViewData["TotalApplications"] = applications.Count;

            return View(applications);
        }

        // GET: LeaseApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseApplication = await _context.LeaseApplications
       .Include(l => l.Tenant)
           .ThenInclude(e => e.User)
       .Include(l => l.Unit)
           .ThenInclude(u => u.Property)
       .Include(l => l.Duration)
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
            var unit = _context.Units
                .Include(u => u.Property)
                .FirstOrDefault(u => u.UnitId == unitId);

            if (unit == null)
            {
                return NotFound();
            }

            var model = new LeaseApplication
            {
                UnitId = unit.UnitId,
                StartDate = DateTime.Today.AddDays(1)
            };

            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");
            ViewBag.UnitNumber = unit.UnitNumber;
            ViewBag.PropertyName = unit.Property.Name;

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
            .ThenInclude(e => e.User)
        .Include(l => l.Unit)
            .ThenInclude(u => u.Property)
        .Include(l => l.Duration)
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
