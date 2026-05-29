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
                    .Count(u => u.AvailabilityStatus != "Occupied"),
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

        var resolved = requests.Where(r => r.ResolvedTime.HasValue).ToList();

        var report = new
        {
            TotalRequests = requests.Count,
            PendingRequests = requests.Count(r => r.Status == "Pending"),
            InProgressRequests = requests.Count(r => r.Status == "In Progress"),
            ResolvedRequests = requests.Count(r => r.Status == "Resolved"),
            ClosedRequests = requests.Count(r => r.Status == "Closed"),
            AverageResolutionDays = resolved.Any()
                ? resolved.Average(r => (r.ResolvedTime!.Value - r.RequestDate).TotalDays)
                : 0,
            AverageResolutionHours = resolved.Any()
                ? resolved.Average(r => (r.ResolvedTime!.Value - r.RequestDate).TotalHours)
                : 0,
            ByType = requests
                .GroupBy(r => r.Skill.Name)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToList(),
            ResolvedRequestDetails = resolved
                .OrderByDescending(r => r.ResolvedTime)
                .Select(r => new
                {
                    RequestId      = r.RequestId,
                    SkillType      = r.Skill.Name,
                    RequestDate    = r.RequestDate,
                    ResolvedDate   = r.ResolvedTime!.Value,
                    DaysToResolve  = Math.Round((r.ResolvedTime!.Value - r.RequestDate).TotalDays, 1),
                    HoursToResolve = Math.Round((r.ResolvedTime!.Value - r.RequestDate).TotalHours, 1),
                    TimeToResolve  = FormatDuration(r.ResolvedTime!.Value - r.RequestDate)
                })
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
                    .ThenInclude(u => u.Property)
            .ToListAsync();

        // 3% per annum Islamic profit rate (Ta'widh — AAOIFI / CBB Bahrain compliant)
        const double islamicProfitRate = 0.03;

        var overdueList = payments
            .Where(p => p.Status != "Paid" && p.EndDate < DateTime.UtcNow)
            .Select(p =>
            {
                var daysOverdue  = (int)(DateTime.UtcNow - p.EndDate).TotalDays;
                var profit       = Math.Round((double)p.Amount * islamicProfitRate * (daysOverdue / 365.0), 3);
                var profitDec    = (decimal)profit;
                return new
                {
                    TenantName       = p.Lease.Tenant.User.FullName,
                    UnitNumber       = p.Lease.Unit.UnitNumber,
                    PropertyName     = p.Lease.Unit.Property.Name,
                    AmountDue        = p.Amount,
                    DueDate          = p.EndDate,
                    DaysOverdue      = daysOverdue,
                    ProfitAmount     = profitDec,
                    TotalAmountDue   = p.Amount + profitDec
                };
            })
            .ToList();

        var report = new
        {
            TotalCollected = payments
                .Where(p => p.Status == "Paid")
                .Sum(p => p.Amount),
            TotalOutstanding = payments
                .Where(p => p.Status != "Paid")
                .Sum(p => p.Amount),
            OverdueCount       = overdueList.Count,
            IslamicProfitRate  = islamicProfitRate,
            OverduePayments    = overdueList
        };

        return Ok(report);
    }

    private static string FormatDuration(TimeSpan ts)
    {
        // Standard HH:MM:SS — hours wrap 0-23
        return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
    }
}