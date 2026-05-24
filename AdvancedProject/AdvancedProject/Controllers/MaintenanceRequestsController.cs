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
    public class MaintenanceRequestsController : Controller
    {
        private readonly APContext _context;
        private readonly IHubContext<NotificationHub> _notifHub;
        private readonly IHubContext<MaintenanceBoardHub> _boardHub;

        public MaintenanceRequestsController(
            APContext context,
            IHubContext<NotificationHub> notifHub,
            IHubContext<MaintenanceBoardHub> boardHub)
        {
            _context = context;
            _notifHub = notifHub;
            _boardHub = boardHub;
        }

        // Pushes a saved Notification to the recipient's browser via SignalR.
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

        // Reloads the request with all navigation properties and broadcasts the board event.
        private async Task BroadcastBoardUpdateAsync(int requestId, string eventType)
        {
            var req = await _context.MaintenanceRequests
                .Include(m => m.Skill)
                .Include(m => m.User)
                .Include(m => m.Unit).ThenInclude(u => u!.Property)
                .Include(m => m.AssignedStaff).ThenInclude(s => s!.User)
                .FirstOrDefaultAsync(m => m.RequestId == requestId);

            if (req == null) return;

            await _boardHub.Clients.Group("maintenance-board").SendAsync("BoardUpdated", new
            {
                eventType,
                requestId    = req.RequestId,
                status       = req.Status,
                priority     = req.Priority,
                skillName    = req.Skill?.Name ?? "—",
                unitNumber   = req.Unit?.UnitNumber ?? "—",
                propertyName = req.Unit?.Property?.Name ?? "—",
                submittedBy  = req.User?.Username ?? "—",
                assignedStaff = req.AssignedStaff?.User?.Username ?? "Unassigned",
                requestDate  = req.RequestDate.ToString("dd MMM yyyy")
            });
        }

        private async Task<MaintenanceStaff?> GetAvailableStaff(int skillId)
        {
            return await _context.MaintenanceStaffs
                .Include(s => s.Skills)
                .Where(s =>
                    s.AvailabilityStatus == "Available" &&
                    s.Skills.Any(sk => sk.SkillId == skillId))
                .FirstOrDefaultAsync();
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

            // Tenants only see their own requests; staff only see assigned requests
            if (!User.IsInRole("PropertyManager"))
            {
                var currentUserEmail = User.Identity!.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser != null)
                {
                    if (User.IsInRole("MaintenanceStaff"))
                    {
                        var staff = await _context.MaintenanceStaffs.FirstOrDefaultAsync(s => s.UserId == currentUser.UserId);
                        if (staff != null)
                            query = query.Where(m => m.AssignedStaffId == staff.StaffId);
                        else
                            query = query.Where(m => false);
                    }
                    else // Tenant
                    {
                        query = query.Where(m => m.UserId == currentUser.UserId);
                    }
                }
            }

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
        .ThenInclude(s => s.User)
    .Include(m => m.Skill)
    .Include(m => m.User)
    .Include(m => m.Unit)
        .ThenInclude(u => u.Property)
    .FirstOrDefaultAsync(m => m.RequestId == id);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Create
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

            var currentUserEmail = User.Identity!.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);

            if (currentUser == null) return Unauthorized();

            if (User.IsInRole("PropertyManager"))
            {
                // Manager: list every active unit in the system
                var allUnits = await _context.Units
                    .Include(u => u.Property)
                    .Where(u => u.IsActive)
                    .Select(u => new
                    {
                        u.UnitId,
                        DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                    })
                    .ToListAsync();

                ViewBag.HasActiveLease = true;
                ViewData["UnitId"] = new SelectList(allUnits, "UnitId", "DisplayName");
                ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name");
                return View();
            }
            else
            {
                // Tenant: only units from their active leases
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == currentUser.UserId);

                if (tenant == null)
                {
                    ViewBag.HasActiveLease = false;
                    return View();
                }

                var activeLeases = await _context.Leases
                    .Include(l => l.Unit).ThenInclude(u => u.Property)
                    .Where(l => l.TenantId == tenant.TenantId && l.Status == "Active")
                    .ToListAsync();

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
        }

        // POST: MaintenanceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,SkillId,Priority,Notes")] MaintenanceRequest maintenanceRequest)
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

            var currentUserEmail = User.Identity!.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);

            if (currentUser == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                maintenanceRequest.UserId = currentUser.UserId;
                maintenanceRequest.RequestDate = DateTime.Now;
                maintenanceRequest.Status = "Pending";

                // Get available staff based on skill
                var staff = await GetAvailableStaff(maintenanceRequest.SkillId);

                // fallback if no matching skill
                if (staff == null)
                {
                    staff = await _context.MaintenanceStaffs
                        .Where(s => s.AvailabilityStatus == "Available")
                        .FirstOrDefaultAsync();
                }

                // assign staff
                maintenanceRequest.AssignedStaffId = staff?.StaffId;

                if (staff != null)
                    staff.AvailabilityStatus = "Busy";

                _context.Add(maintenanceRequest);
                await _context.SaveChangesAsync();

                // Notify manager when a tenant submits
                Notification? managerNotif = null;
                if (!User.IsInRole("PropertyManager"))
                {
                    managerNotif = new Notification
                    {
                        UserId = 1,
                        Title = "New Maintenance Request",
                        Message = "A new maintenance request has been submitted.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(managerNotif);
                }

                // Notify the assigned staff member
                Notification? staffNotif = null;
                if (staff != null)
                {
                    staffNotif = new Notification
                    {
                        UserId = staff.UserId,
                        Title = "New Maintenance Request",
                        Message = "You have been assigned a new maintenance request.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(staffNotif);
                }

                await _context.SaveChangesAsync();

                // Push notifications via SignalR (to manager and staff who have the app open in other tabs)
                if (managerNotif != null) await PushNotificationAsync(managerNotif);
                if (staffNotif != null)   await PushNotificationAsync(staffNotif);

                // Broadcast the new request to the maintenance board
                await BroadcastBoardUpdateAsync(maintenanceRequest.RequestId, "created");

                // Carry a toast payload across the redirect so the submitting user sees real-time feedback
                TempData["ToastTitle"]   = "Request Submitted";
                TempData["ToastMessage"] = $"Your maintenance request #{maintenanceRequest.RequestId} has been submitted successfully.";
                TempData["ToastType"]    = "Maintenance";
                TempData["SuccessMessage"] = "Maintenance Request was created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // reload dropdowns if validation fails
            if (User.IsInRole("PropertyManager"))
            {
                var allUnits = await _context.Units
                    .Include(u => u.Property)
                    .Where(u => u.IsActive)
                    .Select(u => new
                    {
                        u.UnitId,
                        DisplayName = u.UnitNumber + " (" + u.Property.Name + ")"
                    })
                    .ToListAsync();

                ViewData["UnitId"] = new SelectList(allUnits, "UnitId", "DisplayName", maintenanceRequest.UnitId);
            }
            else
            {
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == currentUser.UserId);
                var units = tenant == null
                    ? new List<object>()
                    : (await _context.Leases
                        .Where(l => l.TenantId == tenant.TenantId && l.Status == "Active")
                        .Select(l => new { l.Unit.UnitId, DisplayName = l.Unit.UnitNumber + " (" + l.Unit.Property.Name + ")" })
                        .ToListAsync())
                        .GroupBy(x => x.UnitId).Select(g => g.First())
                        .Cast<object>().ToList();

                ViewData["UnitId"] = new SelectList(units, "UnitId", "DisplayName", maintenanceRequest.UnitId);
            }

            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "Name", maintenanceRequest.SkillId);

            return View(maintenanceRequest);
        }

        // GET: MaintenanceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

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
                        Username = s.User.Username
                    }),
                "StaffId",
                "Username",
                maintenanceRequest.AssignedStaffId
            );

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaintenanceRequest form)
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

            if (id != form.RequestId)
                return NotFound();

            var request = await _context.MaintenanceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var oldAssignedStaffId = request.AssignedStaffId;
                bool managerChangedStaff = (form.AssignedStaffId != oldAssignedStaffId);

                MaintenanceStaff? oldStaff = null;
                if (oldAssignedStaffId != null)
                {
                    oldStaff = await _context.MaintenanceStaffs
                        .FirstOrDefaultAsync(s => s.StaffId == oldAssignedStaffId);
                }

                request.UnitId = form.UnitId;
                request.SkillId = form.SkillId;
                request.Priority = form.Priority;
                request.Status = form.Status;
                request.Notes = form.Notes;

                if (!managerChangedStaff)
                {
                    if (oldStaff != null)
                    {
                        oldStaff.AvailabilityStatus = "Available";
                    }

                    var allStaff = await _context.MaintenanceStaffs
                        .Include(s => s.Skills)
                        .ToListAsync();

                    var newStaff = allStaff.FirstOrDefault(s =>
                        s.AvailabilityStatus == "Available" &&
                        s.Skills.Any(sk => sk.SkillId == request.SkillId));

                    if (newStaff == null)
                    {
                        newStaff = allStaff.FirstOrDefault(s => s.AvailabilityStatus == "Available");
                    }

                    if (newStaff == null)
                    {
                        newStaff = allStaff.FirstOrDefault();
                    }

                    request.AssignedStaffId = newStaff?.StaffId;

                    if (newStaff != null)
                    {
                        newStaff.AvailabilityStatus = "Busy";
                    }
                }
                else
                {
                    if (oldStaff != null)
                    {
                        oldStaff.AvailabilityStatus = "Available";
                    }

                    request.AssignedStaffId = form.AssignedStaffId;

                    if (form.AssignedStaffId != null)
                    {
                        var newStaff = await _context.MaintenanceStaffs
                            .FirstOrDefaultAsync(s => s.StaffId == form.AssignedStaffId);
                        if (newStaff != null)
                        {
                            newStaff.AvailabilityStatus = "Busy";
                        }
                    }
                }

                // Notify tenant
                var tenantNotif = new Notification
                {
                    UserId = request.UserId,
                    Title = "Maintenance Update",
                    Message = $"Your maintenance request #{request.RequestId} has been edited.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(tenantNotif);

                // Notify assigned staff
                Notification? staffNotif = null;
                if (request.AssignedStaffId != null)
                {
                    var assignedStaff = await _context.MaintenanceStaffs
                        .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                    if (assignedStaff != null)
                    {
                        staffNotif = new Notification
                        {
                            UserId = assignedStaff.UserId,
                            Title = "Maintenance Request Edited",
                            Message = $"Your maintenance request #{request.RequestId} has been edited.",
                            NotificationTypeId = 2,
                            CreatedAt = DateTime.Now
                        };
                        _context.Notifications.Add(staffNotif);
                    }
                }

                await _context.SaveChangesAsync();

                // Push notifications via SignalR
                await PushNotificationAsync(tenantNotif);
                if (staffNotif != null) await PushNotificationAsync(staffNotif);

                // Broadcast board update
                await BroadcastBoardUpdateAsync(request.RequestId, "updated");

                TempData["SuccessMessage"] = "Maintenance Request was edited successfully.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

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
                        Username = s.User.Username
                    }),
                "StaffId",
                "Username",
                form.AssignedStaffId
            );

            return View(form);
        }

        // GET: MaintenanceRequests/EditTenant/5
        public async Task<IActionResult> EditTenant(int? id)
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

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
                        Username = s.User.Username
                    }),
                "StaffId",
                "Username",
                maintenanceRequest.AssignedStaffId
            );

            return View(maintenanceRequest);
        }

        // POST: MaintenanceRequests/EditTenant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTenant(int id, MaintenanceRequest form)
        {
            if (User.IsInRole("MaintenanceStaff")) return Forbid();

            if (id != form.RequestId)
                return NotFound();

            var request = await _context.MaintenanceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                MaintenanceStaff? oldStaff = null;
                if (request.AssignedStaffId != null)
                {
                    oldStaff = await _context.MaintenanceStaffs
                        .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                }
                if (oldStaff != null)
                {
                    oldStaff.AvailabilityStatus = "Available";
                }

                request.UnitId = form.UnitId;
                request.SkillId = form.SkillId;
                request.Priority = form.Priority;
                request.Status = form.Status;
                request.Notes = form.Notes;

                var allStaff = await _context.MaintenanceStaffs
                    .Include(s => s.Skills)
                    .ToListAsync();

                var newStaff = allStaff.FirstOrDefault(s =>
                    s.AvailabilityStatus == "Available" &&
                    s.Skills.Any(sk => sk.SkillId == request.SkillId));

                if (newStaff == null)
                {
                    newStaff = allStaff.FirstOrDefault(s => s.AvailabilityStatus == "Available");
                }

                if (newStaff == null)
                {
                    newStaff = allStaff.FirstOrDefault();
                }

                request.AssignedStaffId = newStaff?.StaffId;
                if (newStaff != null)
                {
                    newStaff.AvailabilityStatus = "Busy";
                }

                // Notify tenant
                var tenantNotif = new Notification
                {
                    UserId = request.UserId,
                    Title = "Maintenance Update",
                    Message = $"Your maintenance request #{request.RequestId} has been edited.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(tenantNotif);

                // Notify assigned staff
                Notification? staffNotif = null;
                if (request.AssignedStaffId != null)
                {
                    var assignedStaff = await _context.MaintenanceStaffs
                        .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                    if (assignedStaff != null)
                    {
                        staffNotif = new Notification
                        {
                            UserId = assignedStaff.UserId,
                            Title = "Maintenance Request Edited",
                            Message = $"Your maintenance request #{request.RequestId} has been edited.",
                            NotificationTypeId = 2,
                            CreatedAt = DateTime.Now
                        };
                        _context.Notifications.Add(staffNotif);
                    }
                }

                await _context.SaveChangesAsync();

                // Push notifications via SignalR
                await PushNotificationAsync(tenantNotif);
                if (staffNotif != null) await PushNotificationAsync(staffNotif);

                // Broadcast board update
                await BroadcastBoardUpdateAsync(request.RequestId, "updated");

                return RedirectToAction(nameof(Index));
            }

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
                        Username = s.User.Username
                    }),
                "StaffId",
                "Username",
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
          .ThenInclude(s => s.User)
      .Include(m => m.Skill)
      .Include(m => m.User)
      .Include(m => m.Unit)
          .ThenInclude(u => u.Property)
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
            TempData["SuccessMessage"] = "Maintenance Request was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkInProgress(int id)
        {
            var request = await _context.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "In Progress";
            request.InProgressTime = DateTime.Now;

            var tenantNotif = new Notification
            {
                UserId = request.UserId,
                Title = "Request In Progress",
                Message = $"Your maintenance request #{request.RequestId} is now being worked on.",
                NotificationTypeId = 2,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(tenantNotif);

            Notification? otherNotif = null;
            if (User.IsInRole("PropertyManager") && request.AssignedStaffId != null)
            {
                var assignedStaff = await _context.MaintenanceStaffs
                    .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                if (assignedStaff != null)
                {
                    otherNotif = new Notification
                    {
                        UserId = assignedStaff.UserId,
                        Title = "Request Marked In Progress",
                        Message = $"Maintenance request #{request.RequestId} has been marked as In Progress.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(otherNotif);
                }
            }
            else if (User.IsInRole("MaintenanceStaff"))
            {
                otherNotif = new Notification
                {
                    UserId = 1,
                    Title = "Request In Progress",
                    Message = $"Maintenance request #{request.RequestId} has been marked as In Progress by staff.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(otherNotif);
            }

            await _context.SaveChangesAsync();

            await PushNotificationAsync(tenantNotif);
            if (otherNotif != null) await PushNotificationAsync(otherNotif);
            await BroadcastBoardUpdateAsync(id, "updated");

            TempData["ToastTitle"]   = "Status Updated";
            TempData["ToastMessage"] = $"Request #{id} is now In Progress.";
            TempData["ToastType"]    = "Maintenance";
            TempData["SuccessMessage"] = "Request was marked as In Progress successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCancelled(int id)
        {
            var request = await _context.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Cancelled";

            var tenantNotif = new Notification
            {
                UserId = request.UserId,
                Title = "Request Cancelled",
                Message = $"Your maintenance request #{request.RequestId} has been cancelled.",
                NotificationTypeId = 2,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(tenantNotif);

            Notification? otherNotif = null;
            if (request.AssignedStaffId != null)
            {
                var assignedStaff = await _context.MaintenanceStaffs
                    .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                if (assignedStaff != null)
                {
                    otherNotif = new Notification
                    {
                        UserId = assignedStaff.UserId,
                        Title = "Request Cancelled",
                        Message = $"Maintenance request #{request.RequestId} has been cancelled.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(otherNotif);
                }
            }

            if (User.IsInRole("MaintenanceStaff"))
            {
                var managerNotif = new Notification
                {
                    UserId = 1,
                    Title = "Request Cancelled",
                    Message = $"Maintenance request #{request.RequestId} was cancelled by staff.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(managerNotif);
                await _context.SaveChangesAsync();
                await PushNotificationAsync(managerNotif);
            }
            else
            {
                await _context.SaveChangesAsync();
            }

            await PushNotificationAsync(tenantNotif);
            if (otherNotif != null) await PushNotificationAsync(otherNotif);
            await BroadcastBoardUpdateAsync(id, "updated");

            TempData["ToastTitle"]   = "Request Cancelled";
            TempData["ToastMessage"] = $"Request #{id} has been cancelled.";
            TempData["ToastType"]    = "Maintenance";
            TempData["SuccessMessage"] = "Request was marked as Cancelled successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkResolved(int id)
        {
            var request = await _context.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Resolved";
            request.ResolvedTime = DateTime.Now;

            var tenantNotif = new Notification
            {
                UserId = request.UserId,
                Title = "Request Resolved",
                Message = $"Your maintenance request #{request.RequestId} has been resolved.",
                NotificationTypeId = 2,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(tenantNotif);

            Notification? otherNotif = null;
            if (User.IsInRole("PropertyManager") && request.AssignedStaffId != null)
            {
                var assignedStaff = await _context.MaintenanceStaffs
                    .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                if (assignedStaff != null)
                {
                    otherNotif = new Notification
                    {
                        UserId = assignedStaff.UserId,
                        Title = "Request Resolved",
                        Message = $"Maintenance request #{request.RequestId} has been marked as resolved.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(otherNotif);
                }
            }
            else if (User.IsInRole("MaintenanceStaff"))
            {
                otherNotif = new Notification
                {
                    UserId = 1,
                    Title = "Request Resolved",
                    Message = $"Maintenance request #{request.RequestId} has been resolved by staff.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(otherNotif);
            }

            await _context.SaveChangesAsync();

            await PushNotificationAsync(tenantNotif);
            if (otherNotif != null) await PushNotificationAsync(otherNotif);
            await BroadcastBoardUpdateAsync(id, "updated");

            TempData["ToastTitle"]   = "Request Resolved";
            TempData["ToastMessage"] = $"Request #{id} has been marked as resolved.";
            TempData["ToastType"]    = "Maintenance";
            TempData["SuccessMessage"] = "Request was marked as Resolved successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkClosed(int id)
        {
            var request = await _context.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Closed";
            request.ClosedTime = DateTime.Now;

            var tenantNotif = new Notification
            {
                UserId = request.UserId,
                Title = "Request Closed",
                Message = $"Your maintenance request #{request.RequestId} has been closed.",
                NotificationTypeId = 2,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(tenantNotif);

            Notification? otherNotif = null;
            if (User.IsInRole("PropertyManager") && request.AssignedStaffId != null)
            {
                var assignedStaff = await _context.MaintenanceStaffs
                    .FirstOrDefaultAsync(s => s.StaffId == request.AssignedStaffId);
                if (assignedStaff != null)
                {
                    otherNotif = new Notification
                    {
                        UserId = assignedStaff.UserId,
                        Title = "Request Closed",
                        Message = $"Maintenance request #{request.RequestId} has been closed.",
                        NotificationTypeId = 2,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(otherNotif);
                }
            }
            else if (User.IsInRole("MaintenanceStaff"))
            {
                otherNotif = new Notification
                {
                    UserId = 1,
                    Title = "Request Closed",
                    Message = $"Maintenance request #{request.RequestId} has been closed by staff.",
                    NotificationTypeId = 2,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(otherNotif);
            }

            await _context.SaveChangesAsync();

            await PushNotificationAsync(tenantNotif);
            if (otherNotif != null) await PushNotificationAsync(otherNotif);
            await BroadcastBoardUpdateAsync(id, "updated");

            TempData["ToastTitle"]   = "Request Closed";
            TempData["ToastMessage"] = $"Request #{id} has been closed.";
            TempData["ToastType"]    = "Maintenance";
            TempData["SuccessMessage"] = "Request was marked as Closed successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool MaintenanceRequestExists(int id)
        {
            return _context.MaintenanceRequests.Any(e => e.RequestId == id);
        }
    }
}
