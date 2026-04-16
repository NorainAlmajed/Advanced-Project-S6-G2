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
    public class PaymentsController : Controller
    {
        private readonly APContext _context;

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

        public PaymentsController(APContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, string dateFilter)
        {
            var paymentsQuery = _context.Payments
                .Include(p => p.Lease)
                .Include(p => p.PaymentFrequency)
                .Include(p => p.PaymentMethod)
                .AsQueryable();

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
                    paymentsQuery = paymentsQuery.OrderByDescending(p => p.StartDate);
                }
                else
                {
                    paymentsQuery = paymentsQuery.OrderBy(p => p.StartDate);
                }
            }
            else
            {
                paymentsQuery = paymentsQuery.OrderByDescending(p => p.StartDate); // default starts from 1
            }

            var payments = await paymentsQuery.ToListAsync();

            ViewData["CurrentSearchTerm"] = searchTerm;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentDateFilter"] = dateFilter;
            ViewData["TotalPayments"] = payments.Count;

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
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaseId,Status,PaymentMethodId,PaymentFrequencyId, StartDate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var lease = await _context.Leases.Include(l => l.Duration).FirstOrDefaultAsync(l => l.LeaseId == payment.LeaseId);

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
                return RedirectToAction(nameof(Index));
            }

            return View(payment);
        }

        // GET: Payments/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,StartDate,Status,PaymentMethodId,PaymentFrequencyId")] Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();

            var existing = await _context.Payments
                .Include(p => p.Lease)
                .ThenInclude(l => l.Duration)
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

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Payments/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
