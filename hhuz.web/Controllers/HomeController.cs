using hhuz.Models;
using hhuz.web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Public ko'rish uchun oxirgi vakansiyalar
        var positions = await _context.Positions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(6)
            .ToListAsync();

        // Oxirgi 24 soatda yaratilgan CV lar
        var last24Hours = DateTime.UtcNow.AddHours(-24);

        var newCvsLast24Hours = await _context.Cvs
            .AsNoTracking()
            .CountAsync(x => x.CreatedAt >= last24Hours);

        ViewBag.Positions = positions;
        ViewBag.NewCvsLast24Hours = newCvsLast24Hours;

        // Umumiy public statistikalar
        ViewBag.TotalPositions = await _context.Positions
            .AsNoTracking()
            .CountAsync();

        ViewBag.TotalUsers = await _context.Users
            .AsNoTracking()
            .CountAsync();

        return View();
    }
}