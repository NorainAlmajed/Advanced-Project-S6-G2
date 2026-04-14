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
    public class UnitsController : Controller
    {
        private readonly APContext _context;

        public UnitsController(APContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index(
     int? id,
     string searchString,
     string statusFilter,
     string typeFilter,
     string priceFilter)
        {
            var query = _context.Units
                .Include(u => u.Property)
                .Include(u => u.Amenities)
                .AsQueryable();

            if (id != null)
            {
                query = query.Where(u => u.PropertyId == id);

                var property = await _context.Properties.FindAsync(id);
                if (property != null)
                {
                    ViewBag.PropertyName = property.Name;
                }
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(u =>
                    u.UnitNumber.Contains(searchString) ||
                    u.Type.Contains(searchString) ||
                    u.Property.Name.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                query = query.Where(u => u.AvailabilityStatus == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                query = query.Where(u => u.Type == typeFilter);
            }

            if (!string.IsNullOrWhiteSpace(priceFilter))
            {
                switch (priceFilter)
                {
                    case "Under300":
                        query = query.Where(u => u.RentAmount < 300);
                        break;

                    case "300to400":
                        query = query.Where(u => u.RentAmount >= 300 && u.RentAmount <= 400);
                        break;

                    case "401to500":
                        query = query.Where(u => u.RentAmount > 400 && u.RentAmount <= 500);
                        break;

                    case "Above500":
                        query = query.Where(u => u.RentAmount > 500);
                        break;
                }
            }

            var typeQuery = _context.Units.AsQueryable();

            if (id != null)
            {
                typeQuery = typeQuery.Where(u => u.PropertyId == id);
            }

            var unitTypes = await typeQuery
                .Select(u => u.Type)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            ViewBag.TypeList = new SelectList(unitTypes, typeFilter);
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = string.IsNullOrWhiteSpace(statusFilter) ? "All" : statusFilter;
            ViewBag.CurrentType = typeFilter;
            ViewBag.CurrentPrice = priceFilter;
            ViewBag.PropertyId = id;

            var units = await query.ToListAsync();

            return View(units);
        }



        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id, int? propertyId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.Amenities)
                .FirstOrDefaultAsync(m => m.UnitId == id);

            if (unit == null)
            {
                return NotFound();
            }
            ViewBag.PropertyId = propertyId;
            return View(unit);
        }

        // GET: Units/Create
        public IActionResult Create()
        {
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId");
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UnitId,PropertyId,UnitNumber,Type,SizeSqFt,RentAmount,AvailabilityStatus,CreatedAt")] Unit unit)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(unit);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId", unit.PropertyId);
        //    return View(unit);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,PropertyId,UnitNumber,Type,SizeSqFt,RentAmount,AvailabilityStatus")] Unit unit)
        {
            unit.CreatedAt = DateTime.Now;

            ModelState.Remove("Property");
            ModelState.Remove("Amenities");
            ModelState.Remove("LeaseApplications");
            ModelState.Remove("Leases");
            ModelState.Remove("MaintenanceRequests");

            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId", unit.PropertyId);
            return View(unit);
        }



        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId", unit.PropertyId);
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UnitId,PropertyId,UnitNumber,Type,SizeSqFt,RentAmount,AvailabilityStatus,CreatedAt")] Unit unit)
        //{
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UnitId,PropertyId,UnitNumber,Type,SizeSqFt,RentAmount,AvailabilityStatus,CreatedAt")] Unit unit) 
        //{
        //    if (id != unit.UnitId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(unit);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UnitExists(unit.UnitId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId", unit.PropertyId);
        //    return View(unit);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnitId,PropertyId,UnitNumber,Type,SizeSqFt,RentAmount,AvailabilityStatus,CreatedAt")] Unit unit)
        {
            if (id != unit.UnitId)
            {
                return NotFound();
            }

            ModelState.Remove("Property");
            ModelState.Remove("Amenities");
            ModelState.Remove("LeaseApplications");
            ModelState.Remove("Leases");
            ModelState.Remove("MaintenanceRequests");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.UnitId))
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

            ViewData["PropertyId"] = new SelectList(_context.Properties, "PropertyId", "PropertyId", unit.PropertyId);
            return View(unit);
        }
        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Property)
                .FirstOrDefaultAsync(m => m.UnitId == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return _context.Units.Any(e => e.UnitId == id);
        }
    }
}
