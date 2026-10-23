using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Data;
using SSD_Lab1.Models;

namespace SSD_Lab1.Controllers;

[Authorize]
public class MessageBoardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessageBoardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _context.BoardMessages
            .OrderByDescending(message => message.CreatedAtUtc)
            .ToListAsync();
        return View(messages);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Content")] BoardMessage input)
    {
        if (!ModelState.IsValid)
        {
            var messages = await _context.BoardMessages.OrderByDescending(message => message.CreatedAtUtc).ToListAsync();
            return View("Index", messages);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var displayName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(part => !string.IsNullOrWhiteSpace(part)));
        if (string.IsNullOrWhiteSpace(displayName)) displayName = user.Email ?? "Member";

        _context.BoardMessages.Add(new BoardMessage
        {
            Content = input.Content.Trim(),
            AuthorId = user.Id,
            AuthorName = displayName,
            CreatedAtUtc = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
