using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectAPI.Data;

namespace AdvancedProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "PropertyManager")]
public class ReportsController : ControllerBase
{
    private readonly APContext _context;

    public ReportsController(APContext context)
    {
        _context = context;
    }

    // GET api/reports/occupancy
    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancyReport()
    {
        var report = await _context.Properties
            .Include(p => p.Units)
            .Select(p => new
            {
                BuildingName = p.Name,
                TotalUnits = p.Units.Count,
                OccupiedUnits = p.Units
                    .Count(u => u.AvailabilityStatus == "Occupied"),
                VacantUnits = p.Units
                    .Count(u => u.AvailabilityStatus == "Vacant"),
                OccupancyRate = p.Units.Count == 0 ? 0 :
                    (double)p.Units.Count(u =>
                        u.AvailabilityStatus == "Occupied") / p.Units.Count * 100
            })
            .ToListAsync();

        return Ok(report);
    }

    // GET api/reports/maintenance
    [HttpGet("maintenance")]
    public async Task<IActionResult> GetMaintenanceReport()
    {
        var requests = await _context.MaintenanceRequests
            .Include(m => m.Skill)
            .ToListAsync();

        var report = new
        {
            TotalRequests = requests.Count,
            PendingRequests = requests
                .Count(r => r.Status == "Pending"),
            InProgressRequests = requests
                .Count(r => r.Status == "In Progress"),
            ResolvedRequests = requests
                .Count(r => r.Status == "Resolved"),
            AverageResolutionDays = requests
                .Where(r => r.ResolvedTime.HasValue)
                .Select(r => (r.ResolvedTime!.Value -
                    r.RequestDate).TotalDays)
                .DefaultIfEmpty(0)
                .Average(),
            ByType = requests
                .GroupBy(r => r.Skill.Name)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToList()
        };

        return Ok(report);
    }

    // GET api/reports/payments
    [HttpGet("payments")]
    public async Task<IActionResult> GetPaymentReport()
    {
        var payments = await _context.Payments
            .Include(p => p.Lease)
                .ThenInclude(l => l.Tenant)
                    .ThenInclude(t => t.User)
            .Include(p => p.Lease)
                .ThenInclude(l => l.Unit)
            .ToListAsync();

        var report = new
        {
            TotalCollected = payments
                .Where(p => p.Status == "Paid")
                .Sum(p => p.Amount),
            TotalOutstanding = payments
                .Where(p => p.Status != "Paid")
                .Sum(p => p.Amount),
            OverdueCount = payments
                .Count(p => p.Status != "Paid" &&
                    p.EndDate < DateTime.UtcNow),
            OverduePayments = payments
                .Where(p => p.Status != "Paid" &&
                    p.EndDate < DateTime.UtcNow)
                .Select(p => new
                {
                    TenantName = p.Lease.Tenant.User.FullName,
                    UnitNumber = p.Lease.Unit.UnitNumber,
                    AmountDue = p.Amount,
                    DueDate = p.EndDate
                })
                .ToList()
        };

        return Ok(report);
    }
}