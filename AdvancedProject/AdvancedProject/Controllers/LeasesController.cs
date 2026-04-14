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
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.Leases.Include(l => l.Unit).Include(e => e.Duration).Include(l => l.Tenant).ThenInclude(e => e.User);
            return View(await aPContext.ToListAsync());
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
                .Include(l => l.Unit).Include(e => e.Duration)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Leases/Create
        public async Task<IActionResult> Create(int? unitId)
        {
            if (unitId == null)
            {
                return BadRequest("Unit is required");
            }

            var model = new Lease
            {
                UnitId = unitId.Value,
                StartDate = DateTime.Today.AddDays(1)
            };

            var tenants = _context.Tenants
                .Include(t => t.User)
                .Select(t => new
                {
                    t.TenantId,
                    Username = t.User.Username
                })
                .ToList();

            ViewData["TenantName"] = new SelectList(tenants, "TenantId", "Username");
            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");

            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
                return NotFound();

            ViewBag.UnitNumber = unit.UnitNumber;

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lease lease)
        {
            return Content("POST HIT");
            var unit = await _context.Units.FindAsync(lease.UnitId);
            if (unit == null)
                return Content("Unit NOT FOUND");

            var duration = await _context.Durations.FindAsync(lease.DurationId);
            if (duration == null)
                return Content("Duration not found");

            if (lease.StartDate.Date < DateTime.Today.AddDays(1))
            {
                ModelState.AddModelError("StartDate", "Start date must be after today.");
            }

            if (!ModelState.IsValid)
            {
                var tenants = _context.Tenants.Include(t => t.User)
                    .Select(t => new { t.TenantId, Username = t.User.Username })
                    .ToList();

                ViewData["TenantName"] = new SelectList(tenants, "TenantId", "Username");
                ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");
                ViewBag.UnitNumber = unit.UnitNumber;

                return View(lease);
            }

            lease.Status = "Pending";
            lease.CreatedAt = DateTime.Now;
            lease.EndDate = lease.StartDate.AddMonths(duration.Months).AddDays(-1);
            lease.MonthlyRent = unit.RentAmount;

            _context.Add(lease);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Leases");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaseId,TenantId,UnitId,StartDate,EndDate,MonthlyRent,Status,CreatedAt,TerminationDate,DurationId")] Lease lease)
        {
            if (id != lease.LeaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var duration = await _context.Durations.FindAsync(lease.DurationId);

                    if (duration == null)
                        return Content("Duration not found");

                    lease.EndDate = lease.StartDate.AddMonths(duration.Months).AddDays(-1);

                    _context.Update(lease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseExists(lease.LeaseId))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants.Include(t => t.User)
            .Select(t => new { t.TenantId, Username = t.User.Username }),
            "TenantId", "Username", lease.TenantId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", lease.UnitId);
            return View(lease);
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
