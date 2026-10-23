using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Data;
using SSD_Lab1.Models;

namespace SSD_Lab1.Controllers
{
    [Authorize]
    public class OfficeLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfficeLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OfficeLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfficeLocations.OrderBy(item => item.Name).ToListAsync());
        }

        // GET: OfficeLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeLocation = await _context.OfficeLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officeLocation == null)
            {
                return NotFound();
            }

            return View(officeLocation);
        }

        // GET: OfficeLocations/Create
        [Authorize(Roles = SeedData.SupervisorRole)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfficeLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SeedData.SupervisorRole)]
        public async Task<IActionResult> Create([Bind("Name,Address,City,Province,PostalCode,Phone")] OfficeLocation officeLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(officeLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(officeLocation);
        }

        // GET: OfficeLocations/Edit/5
        [Authorize(Roles = SeedData.SupervisorRole)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeLocation = await _context.OfficeLocations.FindAsync(id);
            if (officeLocation == null)
            {
                return NotFound();
            }
            return View(officeLocation);
        }

        // POST: OfficeLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SeedData.SupervisorRole)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,Province,PostalCode,Phone")] OfficeLocation officeLocation)
        {
            if (id != officeLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(officeLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeLocationExists(officeLocation.Id))
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
            return View(officeLocation);
        }

        // GET: OfficeLocations/Delete/5
        [Authorize(Roles = SeedData.SupervisorRole)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeLocation = await _context.OfficeLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officeLocation == null)
            {
                return NotFound();
            }

            return View(officeLocation);
        }

        // POST: OfficeLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SeedData.SupervisorRole)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officeLocation = await _context.OfficeLocations.FindAsync(id);
            if (officeLocation != null)
            {
                _context.OfficeLocations.Remove(officeLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeLocationExists(int id)
        {
            return _context.OfficeLocations.Any(e => e.Id == id);
        }
    }
}
