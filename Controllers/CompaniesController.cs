// I, Meet Sevani, student number 000924119, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Data;
using SSD_Lab1.Models;

namespace SSD_Lab1.Controllers;

[Authorize]
public class CompaniesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompaniesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Companies.OrderBy(company => company.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FirstOrDefaultAsync(company => company.Id == id);
        return company is null ? NotFound() : View(company);
    }

    [Authorize(Roles = SeedData.SupervisorRole)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SeedData.SupervisorRole)]
    public async Task<IActionResult> Create([Bind("Name,YearsInBusiness,Website,Province")] Company company)
    {
        if (!ModelState.IsValid)
        {
            return View(company);
        }

        _context.Add(company);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = SeedData.SupervisorRole)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FindAsync(id);
        return company is null ? NotFound() : View(company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SeedData.SupervisorRole)]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,YearsInBusiness,Website,Province")] Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(company);
        }

        try
        {
            _context.Update(company);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompanyExists(company.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = SeedData.SupervisorRole)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FirstOrDefaultAsync(company => company.Id == id);
        return company is null ? NotFound() : View(company);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SeedData.SupervisorRole)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company is not null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CompanyExists(int id)
    {
        return _context.Companies.Any(company => company.Id == id);
    }
}
