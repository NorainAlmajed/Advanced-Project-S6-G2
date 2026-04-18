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
        public async Task<IActionResult> Index(string searchTerm, string priorityFilter, string statusFilter, int? typeFilter, string sortOrder)
        {
            var query = _context.MaintenanceRequests
            .Include(m => m.AssignedStaff)
                .ThenInclude(s => s.User)
            .Include(m => m.Skill)
            .Include(m => m.User)
            .Include(m => m.Unit)
                .ThenInclude(u => u.Property)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                query = query.Where(m =>
                    m.RequestId.ToString().Contains(searchTerm) ||
                    (m.Skill != null && m.Skill.Name.Contains(searchTerm)) ||
                   (m.User != null && (m.User.Phone.Contains(searchTerm) )));
            }

            if (!string.IsNullOrWhiteSpace(priorityFilter))
            {
                query = query.Where(m => m.Priority == priorityFilter);
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(m => m.Status == statusFilter);
            }

            if (typeFilter.HasValue)
            {
                query = query.Where(m => m.SkillId == typeFilter.Value);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PriorityFilter = priorityFilter;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.TypeFilter = typeFilter;

            ViewBag.TypeList = new SelectList(
                await _context.Skills.OrderBy(s => s.Name).ToListAsync(),
                "SkillId",
                "Name",
                typeFilter
            );


            ViewBag.SortOrder = sortOrder;

            query = sortOrder switch
            {
                "oldest" => query.OrderBy(m => m.RequestDate),
                "id_asc" => query.OrderBy(m => m.RequestId),
                "id_desc" => query.OrderByDescending(m => m.RequestId),
                _ => query.OrderByDescending(m => m.RequestDate) // latest first
            };


            var requests = await query.ToListAsync();

            return View(requests);
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
                .Include(m => m.User)
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
            int tenantId = 1; // temporary logged-in user

            var activeLeases = _context.Leases
            .Include(l => l.Unit)
            .ThenInclude(u => u.Property)
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .ToList();

            if (!activeLeases.Any())
            {
                ViewBag.HasActiveLease = false;
                return View();
            }

            ViewBag.HasActiveLease = true;

            var units = activeLeases
                .Select(l => new
                {
                    l.Unit.UnitId,
                    DisplayName = l.Unit.UnitNumber + " (" + l.Unit.Property.Name + ")"
                })
                .GroupBy(x => x.UnitId)
                .Select(g => g.First())
                .ToList();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName");
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name");

            return View();
        }

        // POST: MaintenanceRequests/Create
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
                maintenanceRequest.UserId = 2;
                maintenanceRequest.RequestDate = DateTime.Now;
                maintenanceRequest.Status = "Pending";

                var staff = _context.MaintenanceStaffs
                    .Include(s => s.Skills)
                    .Where(s => s.AvailabilityStatus == "Available"
                             && s.Skills.Any(sk => sk.SkillId == maintenanceRequest.SkillId))
                    .FirstOrDefault();

                if (staff == null)
                {
                    staff = _context.MaintenanceStaffs
                        .Where(s => s.AvailabilityStatus == "Available")
                        .FirstOrDefault();
                }

                maintenanceRequest.AssignedStaffId = staff?.StaffId;

                _context.Add(maintenanceRequest);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            int tenantId = 1;

            var units = _context.Leases
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .Select(l => new
                {
                    l.Unit.UnitId,
                    DisplayName = l.Unit.UnitNumber + " (" + l.Unit.Property.Name + ")"
                })
                .ToList()
                .GroupBy(x => x.UnitId)
                .Select(g => g.First())
                .ToList();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", maintenanceRequest.UnitId);

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", maintenanceRequest.SkillId);

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenanceRequest = await _context.MaintenanceRequests
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (maintenanceRequest == null)
                return NotFound();

            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.UserId == maintenanceRequest.UserId);

            if (tenant == null)
            {
                ViewData["UnitId"] = new SelectList(new List<object>(), "UnitId", "DisplayName");
                return View(maintenanceRequest);
            }

            int tenantId = tenant.TenantId;

            var unitIds = await _context.Leases
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .Select(l => l.UnitId)
                .Distinct()
                .ToListAsync();

            var units = await _context.Units
                .Include(u => u.Property) // IMPORTANT FIX
                .Where(u => unitIds.Contains(u.UnitId))
                .Select(u => new
                {
                    u.UnitId,
                    DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                })
                .ToListAsync();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", maintenanceRequest.UnitId);

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", maintenanceRequest.SkillId);

            ViewData["AssignedStaffId"] = new SelectList(
                _context.MaintenanceStaffs
                    .Include(s => s.User)
                    .Select(s => new
                    {
                        s.StaffId,
                        FullName = s.User.FullName
                    }),
                "StaffId",
                "FullName",
                maintenanceRequest.AssignedStaffId
            );

            return View(maintenanceRequest);
        }





        // POST: MaintenanceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaintenanceRequest form)
        {
            if (id != form.RequestId)
                return NotFound();

            var request = await _context.MaintenanceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                request.UnitId = form.UnitId;
                request.SkillId = form.SkillId;
                request.Priority = form.Priority;
                request.Status = form.Status;
                request.AssignedStaffId = form.AssignedStaffId;
                request.Notes = form.Notes;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 🔥 reload Unit dropdown
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.UserId == request.UserId);

            if (tenant != null)
            {
                int tenantId = tenant.TenantId;

                var unitIds = await _context.Leases
                    .Where(l => l.TenantId == tenantId && l.Status == "Active")
                    .Select(l => l.UnitId)
                    .Distinct()
                    .ToListAsync();

                var units = await _context.Units
                    .Include(u => u.Property)
                    .Where(u => unitIds.Contains(u.UnitId))
                    .Select(u => new
                    {
                        u.UnitId,
                        DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                    })
                    .ToListAsync();

                ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", form.UnitId);
            }

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", form.SkillId);

            ViewData["AssignedStaffId"] = new SelectList(
                _context.MaintenanceStaffs
                    .Include(s => s.User)
                    .Select(s => new
                    {
                        s.StaffId,
                        FullName = s.User.FullName
                    }),
                "StaffId",
                "FullName",
                form.AssignedStaffId
            );

            return View(form);
        }





        //New


        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> EditTenant(int? id)
        {
            if (id == null)
                return NotFound();

            var maintenanceRequest = await _context.MaintenanceRequests
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (maintenanceRequest == null)
                return NotFound();

            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.UserId == maintenanceRequest.UserId);

            if (tenant == null)
            {
                ViewData["UnitId"] = new SelectList(new List<object>(), "UnitId", "DisplayName");
                return View(maintenanceRequest);
            }

            int tenantId = tenant.TenantId;

            var unitIds = await _context.Leases
                .Where(l => l.TenantId == tenantId && l.Status == "Active")
                .Select(l => l.UnitId)
                .Distinct()
                .ToListAsync();

            var units = await _context.Units
                .Include(u => u.Property)
                .Where(u => unitIds.Contains(u.UnitId))
                .Select(u => new
                {
                    u.UnitId,
                    DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                })
                .ToListAsync();

            ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", maintenanceRequest.UnitId);

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", maintenanceRequest.SkillId);

            ViewData["AssignedStaffId"] = new SelectList(
                _context.MaintenanceStaffs
                    .Include(s => s.User)
                    .Select(s => new
                    {
                        s.StaffId,
                        FullName = s.User.FullName
                    }),
                "StaffId",
                "FullName",
                maintenanceRequest.AssignedStaffId
            );

            return View(maintenanceRequest);
        }





        // POST: MaintenanceRequests/EditTenant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTenant(int id, MaintenanceRequest form)
        {
            if (id != form.RequestId)
                return NotFound();

            var request = await _context.MaintenanceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                request.UnitId = form.UnitId;
                request.SkillId = form.SkillId;
                request.Priority = form.Priority;
                request.Status = form.Status;
                request.AssignedStaffId = form.AssignedStaffId;
                request.Notes = form.Notes;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // reload Unit dropdown
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.UserId == request.UserId);

            if (tenant != null)
            {
                int tenantId = tenant.TenantId;

                var unitIds = await _context.Leases
                    .Where(l => l.TenantId == tenantId && l.Status == "Active")
                    .Select(l => l.UnitId)
                    .Distinct()
                    .ToListAsync();

                var units = await _context.Units
                    .Include(u => u.Property)
                    .Where(u => unitIds.Contains(u.UnitId))
                    .Select(u => new
                    {
                        u.UnitId,
                        DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                    })
                    .ToListAsync();

                ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", form.UnitId);
            }

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", form.SkillId);

            ViewData["AssignedStaffId"] = new SelectList(
                _context.MaintenanceStaffs
                    .Include(s => s.User)
                    .Select(s => new
                    {
                        s.StaffId,
                        FullName = s.User.FullName
                    }),
                "StaffId",
                "FullName",
                form.AssignedStaffId
            );

            return View(form);
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
                .Include(m => m.User)
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
