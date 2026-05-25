using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AdvancedProject.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AdvancedProject.Controllers
{
    [Authorize]
    public class LeaseApplicationsController : Controller
    {
        private readonly APContext _context;
        private readonly IHubContext<NotificationHub> _notifHub;

        public LeaseApplicationsController(APContext context, IHubContext<NotificationHub> notifHub)
        {
            _context = context;
            _notifHub = notifHub;
        }

        private async Task PushNotificationAsync(Notification notification)
        {
            var typeName = notification.NotificationTypeId switch
            {
                2 => "Maintenance",
                3 => "Payment",
                _ => "Lease"
            };
            await _notifHub.Clients
                .Group($"user-{notification.UserId}")
                .SendAsync("ReceiveNotification", new
                {
                    notificationId = notification.NotificationId,
                    title          = notification.Title,
                    message        = notification.Message,
                    typeName,
                    createdAt      = notification.CreatedAt.ToString("dd MMM yyyy · hh:mm tt")
                });
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

            // Tenants only see their own applications
            if (!User.IsInRole("PropertyManager"))
            {
                var currentUserEmail = User.Identity!.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser != null)
                {
                    var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == currentUser.UserId);
                    if (tenant != null)
                        applicationsQuery = applicationsQuery.Where(l => l.TenantId == tenant.TenantId);
                    else
                        applicationsQuery = applicationsQuery.Where(l => false);
                }
            }

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

            // Auto-reject pending applications whose start date has already passed
            var today = DateTime.Today;
            var overdue = await _context.LeaseApplications
                .Where(a => a.Status == "Pending" && a.StartDate.Date <= today)
                .ToListAsync();

            foreach (var app in overdue)
            {
                app.Status = "Rejected";
                app.RejectTime = DateTime.Now;
            }

            if (overdue.Any())
                await _context.SaveChangesAsync();

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
             .ThenInclude(t => t.User)
         .Include(l => l.Unit)
             .ThenInclude(u => u.Property)
         .Include(l => l.Unit)
             .ThenInclude(u => u.UnitType)
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
                return NotFound();

            if (unit.AvailabilityStatus == "Occupied")
                return RedirectToAction("Index", "Units");

            var model = new LeaseApplication
            {
                UnitId = unit.UnitId,
                StartDate = DateTime.Today.AddDays(1)
            };

            ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months");
            ViewBag.UnitNumber = unit.UnitNumber;
            ViewBag.PropertyName = unit.Property.Name;
            ViewBag.UnitId = unit.UnitId;
            ViewBag.PropertyId = unit.Property.PropertyId;

            return View(model);
        }

        // POST: LeaseApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaseApplication leaseApplication)
        {
            var currentUserEmail = User.Identity!.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            var tenant = currentUser == null ? null
                : await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == currentUser.UserId);

            leaseApplication.TenantId = tenant?.TenantId ?? 0;
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

            var managerNotif = new Notification
            {
                UserId = 1,
                Title = "New Lease Application",
                Message = $"A new lease application #{leaseApplication.ApplicationId} has been submitted.",
                NotificationTypeId = 1,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(managerNotif);
            await _context.SaveChangesAsync();

            await PushNotificationAsync(managerNotif);

            TempData["ToastTitle"]   = "Application Submitted";
            TempData["ToastMessage"] = $"Your lease application #{leaseApplication.ApplicationId} has been submitted successfully.";
            TempData["ToastType"]    = "Lease";
            TempData["SuccessMessage"] = "Lease Application was created successfully.";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,StartDate,DurationId")] LeaseApplication leaseApplication)
        {
            Console.WriteLine("POST HIT ??");

            if (id != leaseApplication.ApplicationId)
            {
                return NotFound();
            }

            var existing = await _context.LeaseApplications.FindAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            // ?? IMPORTANT: remove validation for fields not included in Bind
            ModelState.Remove("Tenant");
            ModelState.Remove("Unit");
            ModelState.Remove("Duration");
            ModelState.Remove("Status");

            // optional validation (keep your logic style)
            if (leaseApplication.StartDate.Date <= DateTime.Today)
            {
                ModelState.AddModelError("StartDate", "Start date must be in the future.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState NOT valid ?");

                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                foreach (var err in errors)
                {
                    Console.WriteLine(err);
                }

                ViewData["DurationId"] = new SelectList(_context.Durations, "DurationId", "Months", leaseApplication.DurationId);
                return View(leaseApplication);
            }

            Console.WriteLine("ModelState valid ?");

            // ? update only allowed fields
            existing.StartDate = leaseApplication.StartDate;
            existing.DurationId = leaseApplication.DurationId;

            var editNotif = new Notification
            {
                UserId = 1,
                Title = "Lease Application Edited",
                Message = $"Lease application #{existing.ApplicationId} has been edited by the tenant.",
                NotificationTypeId = 1,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(editNotif);

            await _context.SaveChangesAsync();

            await PushNotificationAsync(editNotif);

            TempData["ToastTitle"]   = "Application Updated";
            TempData["ToastMessage"] = $"Your lease application #{existing.ApplicationId} has been updated successfully.";
            TempData["ToastType"]    = "Lease";
            TempData["SuccessMessage"] = "Lease Application was edited successfully.";
            return RedirectToAction(nameof(Details), new { id = id });
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
            TempData["SuccessMessage"] = "Lease Application was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }



        //raghad added this on 14May2026

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var application = await _context.LeaseApplications
                .Include(a => a.Unit)
                .Include(a => a.Duration)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.Status == "Approved")
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            application.Status = "Approved";
            application.ApproveTime = DateTime.Now;
            application.RejectTime = null;

            var lease = new Lease
            {
                TenantId = application.TenantId,
                UnitId = application.UnitId,
                StartDate = application.StartDate,
                EndDate = application.StartDate.AddMonths(application.Duration.Months),
                DurationId = application.DurationId,
                MonthlyRent = application.Unit.RentAmount,
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            application.Unit.AvailabilityStatus = "Occupied";

            _context.Leases.Add(lease);

            var approveNotif = new Notification
            {
                UserId = application.Tenant.UserId,
                Title = "Application Approved",
                Message = $"Your lease application #{application.ApplicationId} has been approved.",
                NotificationTypeId = 1,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(approveNotif);

            await _context.SaveChangesAsync();

            await PushNotificationAsync(approveNotif);

            TempData["ToastTitle"]   = "Application Approved";
            TempData["ToastMessage"] = $"Lease application #{application.ApplicationId} has been approved and a lease has been created.";
            TempData["ToastType"]    = "Lease";
            TempData["SuccessMessage"] = "Application was approved successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await _context.LeaseApplications
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.Status == "Rejected")
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            application.Status = "Rejected";
            application.RejectTime = DateTime.Now;
            application.ApproveTime = null;

            var rejectNotif = new Notification
            {
                UserId = application.Tenant.UserId,
                Title = "Application Rejected",
                Message = $"Your lease application #{application.ApplicationId} has been rejected.",
                NotificationTypeId = 1,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(rejectNotif);

            await _context.SaveChangesAsync();

            await PushNotificationAsync(rejectNotif);

            TempData["ToastTitle"]   = "Application Rejected";
            TempData["ToastMessage"] = $"Lease application #{application.ApplicationId} has been rejected.";
            TempData["ToastType"]    = "Lease";
            TempData["SuccessMessage"] = "Application was rejected successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplication(int id)
        {
            var application = await _context.LeaseApplications.FindAsync(id);
            if (application == null) return NotFound();

            if (application.Status == "Pending")
            {
                application.Status = "Cancelled";

                var cancelNotif = new Notification
                {
                    UserId = 1,
                    Title = "Application Cancelled",
                    Message = $"Lease application #{application.ApplicationId} has been cancelled by the tenant.",
                    NotificationTypeId = 1,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(cancelNotif);

                await _context.SaveChangesAsync();

                await PushNotificationAsync(cancelNotif);

                TempData["ToastTitle"]   = "Application Cancelled";
                TempData["ToastMessage"] = $"Your lease application #{application.ApplicationId} has been cancelled.";
                TempData["ToastType"]    = "Lease";
                TempData["SuccessMessage"] = "Application was cancelled successfully.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool LeaseApplicationExists(int id)
        {
            return _context.LeaseApplications.Any(e => e.ApplicationId == id);
        }
    }
}
