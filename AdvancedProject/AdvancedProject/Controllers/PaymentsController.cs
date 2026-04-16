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

        public PaymentsController(APContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var aPContext = _context.Payments.Include(p => p.Lease).Include(p => p.PaymentFrequency).Include(p => p.PaymentMethod);
            return View(await aPContext.ToListAsync());
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

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["LeaseId"] = new SelectList(_context.Leases, "LeaseId", "LeaseId");
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name");
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaseId,Status,PaymentMethodId,PaymentFrequencyId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var lease = await _context.Leases.FindAsync(payment.LeaseId);
                var frequency = await _context.PaymentFrequencies.FindAsync(payment.PaymentFrequencyId);

                if (lease == null || frequency == null)
                {
                    return NotFound();
                }

                payment.StartDate = lease.StartDate;
                payment.EndDate = payment.StartDate.AddDays(7);
                payment.Amount = lease.MonthlyRent * frequency.Frequency;

                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LeaseId"] = new SelectList(_context.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name", payment.PaymentFrequencyId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", payment.PaymentMethodId);

            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["LeaseId"] = new SelectList(_context.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name", payment.PaymentFrequencyId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", payment.PaymentMethodId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,LeaseId,Amount,StartDate,EndDate,Status,PaymentMethodId,PaymentFrequencyId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["LeaseId"] = new SelectList(_context.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewData["PaymentFrequencyId"] = new SelectList(_context.PaymentFrequencies, "PaymentFrequencyId", "Name", payment.PaymentFrequencyId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "Name", payment.PaymentMethodId);
            return View(payment);
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
