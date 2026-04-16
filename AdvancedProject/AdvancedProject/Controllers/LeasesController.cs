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
    public class LeasesController : Controller
    {
        private readonly APContext _context;

        public LeasesController(APContext context)
        {
            _context = context;
        }

        private void LoadDropdowns()
        {
            var tenants = _context.Tenants
                .Include(t => t.User)
                .Select(t => new
                {
                    t.TenantId,
                    Username = t.User.Username
                })
                .ToList();

            ViewData["TenantName"] = new SelectList(tenants, "TenantId", "Username");
            ViewData["UnitNumber"] = new SelectList(
                _context.Units,
                "UnitId",
                "UnitNumber"
            );
        }

        // GET: Leases
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, string dateFilter)
        {
            var leasesQuery = _context.Leases
                .Include(l => l.Tenant)
                    .ThenInclude(t => t.User)
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Property)
                .Include(l => l.Duration)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                leasesQuery = leasesQuery.Where(l =>
                    l.LeaseId.ToString().Contains(searchTerm) ||
                    l.Tenant.User.FullName.Contains(searchTerm) ||
                    l.Unit.UnitNumber.Contains(searchTerm) ||
                    l.Unit.Property.Name.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                leasesQuery = leasesQuery.Where(l => l.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(dateFilter))
            {
                if (dateFilter == "Latest")
                {
                    leasesQuery = leasesQuery.OrderByDescending(l => l.StartDate);
                }
                else
                {
                    leasesQuery = leasesQuery.OrderBy(l => l.StartDate);
                }
            }
            else
            {
                leasesQuery = leasesQuery.OrderByDescending(l => l.StartDate);
            }

            var leases = await leasesQuery.ToListAsync();

            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentDateFilter"] = dateFilter;
            ViewData["TotalLeases"] = leases.Count;

            return View(leases);
        }

        // GET: Leases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Tenant).ThenInclude(e => e.User)
                .Include(l => l.Unit)
                    .ThenInclude(u => u.Property)
                .Include(e => e.Duration).FirstOrDefaultAsync(m => m.LeaseId == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Leases/Create
        public async Task<IActionResult> Create(int unitId)
        {
            var unit = await _context.Units
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.UnitId == unitId);

            if (unit == null)
                return NotFound();

            var model = new Lease
            {
                UnitId = unit.UnitId,
                StartDate = DateTime.Today.AddDays(1)
            };

            ViewBag.UnitNumber = unit.UnitNumber;
            ViewBag.PropertyName = unit.Property.Name;

            ViewData["TenantName"] = new SelectList(
                _context.Tenants.Include(t => t.User)
                    .Select(t => new { t.TenantId, Username = t.User.Username }),
                "TenantId", "Username");

            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lease lease)
        {
            ModelState.Remove("MonthlyRent");
            ModelState.Remove("EndDate");
            ModelState.Remove("Status");
            ModelState.Remove("CreatedAt");

            ModelState.Remove("Tenant");
            ModelState.Remove("Unit");
            ModelState.Remove("Duration");

            if (!ModelState.IsValid)
            {
                var selectedUnit = await _context.Units
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.UnitId == lease.UnitId);

                if (selectedUnit != null)
                {
                    ViewBag.UnitNumber = selectedUnit.UnitNumber;
                    ViewBag.PropertyName = selectedUnit.Property.Name;
                }

                ViewData["TenantName"] = new SelectList(
                    _context.Tenants.Include(t => t.User)
                        .Select(t => new { t.TenantId, Username = t.User.Username }),
                    "TenantId",
                    "Username",
                    lease.TenantId
                );

                ViewData["DurationId"] = new SelectList(
                    _context.Durations,
                    "DurationId",
                    "Months",
                    lease.DurationId
                );

                return View(lease);
            }

            lease.CreatedAt = DateTime.Now;
            lease.Status = "Active";

            var unit = await _context.Units.FindAsync(lease.UnitId);

            if (unit == null)
            {
                ModelState.AddModelError("", "Invalid unit.");
                return View(lease);
            }

            lease.MonthlyRent = unit.RentAmount;

            var duration = await _context.Durations.FindAsync(lease.DurationId);

            if (duration == null)
            {
                ModelState.AddModelError("", "Invalid duration selected.");
                return View(lease);
            }

            lease.EndDate = lease.StartDate.AddMonths(duration.Months).AddDays(-1);

            _context.Leases.Add(lease);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Leases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }
            var tenants = _context.Tenants
            .Include(t => t.User)
            .Select(t => new
            {
                t.TenantId,
                Username = t.User.Username
            })
            .ToList();

            ViewData["TenantId"] = new SelectList(tenants, "TenantId", "Username", lease.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitNumber", lease.UnitId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months", lease.DurationId);
            return View(lease);
        }

        // POST: Leases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaseId,StartDate,DurationId")] Lease lease)
        {
            if (id != lease.LeaseId)
            {
                return NotFound();
            }

            // Remove navigation validation issues
            ModelState.Remove("Tenant");
            ModelState.Remove("Unit");
            ModelState.Remove("Duration");
            ModelState.Remove("Status");
            ModelState.Remove("MonthlyRent");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("EndDate");

            if (!ModelState.IsValid)
            {
                var tenants = _context.Tenants
                    .Include(t => t.User)
                    .Select(t => new
                    {
                        t.TenantId,
                        Username = t.User.Username
                    })
                    .ToList();

                ViewData["TenantId"] = new SelectList(
                    tenants,
                    "TenantId",
                    "Username",
                    lease.TenantId
                );

                ViewData["UnitId"] = new SelectList(
                    _context.Units,
                    "UnitId",
                    "UnitNumber",
                    lease.UnitId
                );

                ViewData["DurationId"] = new SelectList(
                    _context.Durations,
                    "DurationId",
                    "Months",
                    lease.DurationId
                );

                return View(lease);
            }

            var existing = await _context.Leases
                .Include(l => l.Duration)
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (existing == null)
            {
                return NotFound();
            }

            var duration = await _context.Durations.FindAsync(lease.DurationId);

            if (duration == null)
            {
                ModelState.AddModelError("", "Duration not found");
                return View(lease);
            }

            existing.StartDate = lease.StartDate;
            existing.DurationId = lease.DurationId;

            // EndDate = StartDate + Duration.Months
            existing.EndDate = lease.StartDate.AddMonths(duration.Months);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Leases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease != null)
            {
                _context.Leases.Remove(lease);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseExists(int id)
        {
            return _context.Leases.Any(e => e.LeaseId == id);
        }
    }
}
