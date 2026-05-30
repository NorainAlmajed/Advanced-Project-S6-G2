using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using AdvancedProject.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AdvancedProject.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly APContext _context;
        private readonly IHubContext<NotificationHub> _notifHub;

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

        private void PopulateDropdowns()
        {
            ViewData["LeaseId"] = new SelectList(_context.Leases.Where(l => l.Status == "Active"), "LeaseId", "LeaseId");
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name");

            ViewData["StatusList"] = new SelectList(new List<string>
    {
        "Pending", "Paid", "Late"
    });
        }

        private void PopulateEditDropdowns(Payment payment = null)
        {
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", payment?.PaymentMethodId);
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name", payment?.PaymentFrequencyId);

            ViewData["StatusList"] = new SelectList(new List<string>
    {
        "Pending",
        "Paid",
        "Late"
    }, payment?.Status);
        }

        public PaymentsController(APContext context, IHubContext<NotificationHub> notifHub)
        {
            _context = context;
            _notifHub = notifHub;
        }

        // GET: Payments
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, string dateFilter, int page = 1)
        {
            var paymentsQuery = _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Tenant)
                .Include(p => p.PaymentFrequency)
                .Include(p => p.PaymentMethod)
                .AsQueryable();

            // Tenants only see their own payments
            if (!User.IsInRole("PropertyManager"))
            {
                var currentUserEmail = User.Identity!.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser != null)
                {
                    var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == currentUser.UserId);
                    if (tenant != null)
                        paymentsQuery = paymentsQuery.Where(p => p.Lease.TenantId == tenant.TenantId);
                    else
                        paymentsQuery = paymentsQuery.Where(p => false);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                paymentsQuery = paymentsQuery.Where(p =>
                    p.PaymentId.ToString().Contains(searchTerm) ||
                    p.LeaseId.ToString().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                paymentsQuery = paymentsQuery.Where(p => p.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(dateFilter))
            {
                if (dateFilter == "Latest")
                {
                    paymentsQuery = paymentsQuery.OrderByDescending(p => p.PaymentId);
                }
                else
                {
                    paymentsQuery = paymentsQuery.OrderBy(p => p.PaymentId);
                }
            }
            else
            {
                paymentsQuery = paymentsQuery.OrderByDescending(p => p.PaymentId);
            }

            const int pageSize = 10;
            int total = await paymentsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var payments = await paymentsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentDateFilter"] = dateFilter;
            ViewData["TotalPayments"] = total;

            ViewBag.Pagination = new AdvancedProject.ViewModels.PaginationVM
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Action = "Index",
                Controller = "Payments",
                RouteValues = new Dictionary<string, object?>
                {
                    ["searchTerm"] = searchTerm,
                    ["statusFilter"] = statusFilter,
                    ["dateFilter"] = dateFilter
                }
            };

            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Unit)
                        .ThenInclude(u => u.Property)
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Tenant)
                        .ThenInclude(t => t.User)
                .Include(p => p.PaymentFrequency)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            PopulateEditDropdowns(payment);
            return View(payment);
        }

        // GET: Payments/Create
        [Authorize(Roles = "PropertyManager")]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Create([Bind("LeaseId,Status,PaymentMethodId,PaymentFrequencyId, StartDate,GovernorateId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var lease = await _context.Leases
                    .Include(l => l.Duration)
                    .Include(l => l.Tenant)
                    .FirstOrDefaultAsync(l => l.LeaseId == payment.LeaseId);

                var frequency = await _context.PaymentFrequencies.FirstOrDefaultAsync(f => f.PaymentFrequencyId == payment.PaymentFrequencyId);

                if (lease == null || frequency == null)
                {
                    return NotFound();
                }

                if (payment.StartDate < lease.StartDate || payment.StartDate > lease.EndDate)
                {
                    ModelState.AddModelError("StartDate", "Start date must be within lease period.");
                    PopulateDropdowns();
                    return View(payment);
                }

                if (lease.Duration.Months == 6 && frequency.Frequency == 12)
                {
                    ModelState.AddModelError("PaymentFrequencyId", "Yearly Frequency is not allowed for 6 month leases.");
                    PopulateDropdowns();
                    return View(payment);
                }

                payment.EndDate = payment.StartDate.AddDays(7);
                payment.Amount = lease.MonthlyRent * frequency.Frequency;

                _context.Add(payment);
                await _context.SaveChangesAsync();

                var tenantNotif = new Notification
                {
                    UserId = lease.Tenant.UserId,
                    Title = "New Payment Record",
                    Message = $"A new payment record #{payment.PaymentId} has been added to your lease.",
                    NotificationTypeId = 3,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(tenantNotif);
                await _context.SaveChangesAsync();

                await PushNotificationAsync(tenantNotif);

                // Tell the tenant's Payments page to reload so the new payment row appears immediately
                await _notifHub.Clients
                    .Group($"user-{lease.Tenant.UserId}")
                    .SendAsync("PaymentAdded", new { paymentId = payment.PaymentId });

                TempData["ToastTitle"]   = "Payment Created";
                TempData["ToastMessage"] = $"Payment record #{payment.PaymentId} has been created successfully.";
                TempData["ToastType"]    = "Payment";
                TempData["SuccessMessage"] = "Payment was created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(payment);
        }

        // GET: Payments/Edit/5
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            PopulateEditDropdowns(payment);

            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,StartDate,Status,PaymentMethodId,PaymentFrequencyId")] Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();

            var existing = await _context.Payments
                .Include(p => p.Lease).ThenInclude(l => l.Duration)
                .Include(p => p.Lease).ThenInclude(l => l.Tenant)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (existing == null)
                return NotFound();

            var frequency = await _context.PaymentFrequencies
                .FirstOrDefaultAsync(f => f.PaymentFrequencyId == payment.PaymentFrequencyId);

            if (frequency == null)
            {
                ModelState.AddModelError("", "Invalid frequency.");
                PopulateEditDropdowns(payment);
                return View(payment);
            }

            // validation: lease range
            if (payment.StartDate < existing.Lease.StartDate || payment.StartDate > existing.Lease.EndDate)
            {
                ModelState.AddModelError("StartDate", "Start date must be within lease period.");
                PopulateEditDropdowns(payment);
                return View(payment);
            }

            // validation: rule
            if (existing.Lease.Duration.Months == 6 && frequency.Frequency == 12)
            {
                ModelState.AddModelError("PaymentFrequencyId", "Yearly Frequency is not allowed for 6 month leases.");
                PopulateEditDropdowns(payment);
                return View(payment);
            }

            // update
            existing.StartDate = payment.StartDate;
            existing.Status = payment.Status;
            existing.PaymentMethodId = payment.PaymentMethodId;
            existing.PaymentFrequencyId = payment.PaymentFrequencyId;

            existing.EndDate = payment.StartDate.AddDays(7);
            existing.Amount = existing.Lease.MonthlyRent * frequency.Frequency;

            var tenantNotif = new Notification
            {
                UserId = existing.Lease.Tenant.UserId,
                Title = "Payment Updated",
                Message = $"Payment record #{existing.PaymentId} has been updated.",
                NotificationTypeId = 3,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(tenantNotif);

            await _context.SaveChangesAsync();

            await PushNotificationAsync(tenantNotif);

            // Update tenant's Payments page live
            await _notifHub.Clients
                .Group($"user-{tenantNotif.UserId}")
                .SendAsync("PaymentStatusChanged", new
                {
                    paymentId = existing.PaymentId,
                    status    = existing.Status
                });

            TempData["ToastTitle"]   = "Payment Updated";
            TempData["ToastMessage"] = $"Payment record #{existing.PaymentId} has been updated successfully.";
            TempData["ToastType"]    = "Payment";
            TempData["SuccessMessage"] = "Payment was edited successfully.";
            return RedirectToAction(nameof(Details), new { id = id });
        }
        // GET: Payments/Delete/5
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Lease)
                .Include(p => p.PaymentFrequency)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Payment was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
