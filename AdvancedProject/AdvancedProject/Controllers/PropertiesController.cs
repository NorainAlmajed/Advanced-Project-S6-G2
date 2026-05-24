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


namespace AdvancedProject.Controllers
{
    [Authorize(Roles = "PropertyManager,Tenant")]
    public class PropertiesController : Controller
    {
        private readonly APContext _context;

        public PropertiesController(APContext context)
        {
            _context = context;
        }

        // ── Image helper ────────────────────────────────────────────────
        private async Task<(byte[]? data, string? contentType)> ProcessImageUpload(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return (null, null);

            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("imageFile", "Image must not exceed 5 MB.");
                return (null, null);
            }

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("imageFile", "Only JPG, PNG, GIF, or WEBP images are allowed.");
                return (null, null);
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return (ms.ToArray(), file.ContentType);
        }

        // ── Serve property image ─────────────────────────────────────────
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(int? id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property?.ImageData == null || property.ImageContentType == null)
                return NotFound();

            return File(property.ImageData, property.ImageContentType);
        }

        // GET: Properties
        public async Task<IActionResult> Index(string searchString, int? governorateId)
        {
            var properties = _context.Properties
                .Include(p => p.Governorate)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.City.Contains(searchString)
                );
            }

            if (governorateId.HasValue && governorateId.Value > 0)
            {
                properties = properties.Where(p => p.GovernorateId == governorateId.Value);
            }

            ViewBag.Governorates = new SelectList(
                await _context.Governorates.ToListAsync(),
                "GovernorateId",
                "Name",
                governorateId
            );

            return View(await properties.ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @property = await _context.Properties
                .Include(p => p.Governorate)
                .FirstOrDefaultAsync(m => m.PropertyId == id);

            if (@property == null) return NotFound();

            return View(@property);
        }

        // GET: Properties/Create
        [Authorize(Roles = "PropertyManager")]
        public IActionResult Create()
        {
            ViewData["GovernorateId"] = new SelectList(
                _context.Governorates, "GovernorateId", "Name");
            return View();
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Create(
            [Bind("PropertyId,Name,City,Description,Block,Building,Road,GovernorateId")] Property property,
            IFormFile? imageFile)
        {
            if (property.GovernorateId == 0)
                ModelState.AddModelError("GovernorateId", "Please select a governorate.");

            // Image is required on create
            if (imageFile == null || imageFile.Length == 0)
                ModelState.AddModelError("imageFile", "A property image is required.");

            var (imgData, imgType) = await ProcessImageUpload(imageFile);

            if (ModelState.IsValid)
            {
                property.CreatedAt = DateTime.Now;
                property.ImageData = imgData;
                property.ImageContentType = imgType;

                _context.Add(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property was created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["GovernorateId"] = new SelectList(
                _context.Governorates, "GovernorateId", "Name", property.GovernorateId);
            return View(property);
        }

        // GET: Properties/Edit/5
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();

            ViewData["GovernorateId"] = new SelectList(
                _context.Governorates, "GovernorateId", "Name", property.GovernorateId);
            return View(property);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("PropertyId,Name,City,Description,Block,Building,Road,GovernorateId")] Property property,
            IFormFile? imageFile)
        {
            if (id != property.PropertyId) return NotFound();

            var (imgData, imgType) = await ProcessImageUpload(imageFile);

            if (ModelState.IsValid)
            {
                try
                {
                    // Load existing to preserve image if none uploaded
                    var existing = await _context.Properties
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.PropertyId == id);

                    if (imgData != null)
                    {
                        property.ImageData = imgData;
                        property.ImageContentType = imgType;
                    }
                    else if (existing != null)
                    {
                        property.ImageData = existing.ImageData;
                        property.ImageContentType = existing.ImageContentType;
                    }

                    // Preserve CreatedAt
                    property.CreatedAt = existing?.CreatedAt ?? DateTime.Now;
                    property.IsActive = existing?.IsActive ?? true;

                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.PropertyId))
                        return NotFound();
                    else
                        throw;
                }

                TempData["SuccessMessage"] = "Property was edited successfully.";
                return RedirectToAction(nameof(Details), new { id = property.PropertyId });
            }

            ViewData["GovernorateId"] = new SelectList(
                _context.Governorates, "GovernorateId", "Name", property.GovernorateId);
            return View(property);
        }

        // GET: Properties/Delete/5
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @property = await _context.Properties
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (@property == null) return NotFound();

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Properties.FindAsync(id);
            if (@property != null)
                _context.Properties.Remove(@property);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Property was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Units)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null) return NotFound();

            property.IsActive = false;
            foreach (var unit in property.Units)
                unit.IsActive = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }
    }
}
