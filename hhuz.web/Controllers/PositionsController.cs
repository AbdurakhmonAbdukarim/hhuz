using System.Security.Claims;
using hhuz.Dto;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Controllers;

[Authorize]
public class PositionsController : Controller
{
    private readonly PositionService _positionService;
    private readonly DiscussionService _discussionService;

    public PositionsController(
        PositionService positionService,
        DiscussionService discussionService)
    {
        _positionService = positionService;
        _discussionService = discussionService;
    }

    [HttpGet("/positions")]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var positions = await _positionService.GetAllAsync();
        return View(positions);
    }

    [HttpGet("/Positions/Create")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("/Positions")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePosition(PositionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View("Create", dto);

        var position = new Positions
        {
            Title = dto.Title,
            ShortDescription = dto.ShortDescription,
            MaxProjects = dto.MaxProjects
        };

        try
        {
            await _positionService.CreateAsync(position);
            TempData["Success"] = "Position created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error creating position: {ex.Message}");
            return View("Create", dto);
        }
    }

    [HttpGet("/Positions/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        var position = await _positionService.GetByIdAsync(id);
        if (position == null)
            return NotFound();

        var discussions = await _discussionService.GetByPositionAsync(id);
        ViewBag.PositionId = id;

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        var viewModel = new PositionViewDto
        {
            Position = position,
            Discussions = discussions.Select(d => new DiscussionPostDto
            {
                Id = d.Id,
                Content = d.Content,
                UserName = d.User.Username,
                CreatedAt = d.CreatedAt,
                IsAuthor = d.UserId == userId
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet("/Positions/Edit/{id}")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    public async Task<IActionResult> Edit(string id)
    {
        var pos = await _positionService.GetByIdAsync(id);
        if (pos == null)
            return NotFound();

        var dto = new PositionUpdateDto
        {
            Title = pos.Title,
            ShortDescription = pos.ShortDescription,
            MaxProjects = pos.MaxProjects,
            Version = pos.Version
        };

        return View(dto);
    }

    [HttpPost("/Positions/Edit/{id}")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, PositionUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var position = new Positions
        {
            Id = id,
            Title = dto.Title,
            ShortDescription = dto.ShortDescription,
            MaxProjects = dto.MaxProjects
        };

        try
        {
            await _positionService.UpdateAsync(position, dto.Version);
            TempData["Success"] = "Position updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("",
                "This position was modified by another user. Please reload and try again.");
            return View(dto);
        }
    }

    [HttpPost("/Positions/DeleteBulk")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBulk([FromForm] List<string> ids)
    {
        if (ids == null || ids.Count == 0)
            return BadRequest("No positions selected");

        var count = await _positionService.DeleteManyAsync(ids);

        TempData["Success"] = $"{count} position(s) deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}