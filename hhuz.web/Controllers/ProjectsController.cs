using System.Security.Claims;
using hhuz.Dto;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    private readonly ProjectService _service;

    public ProjectsController(ProjectService projectService)
    {
        _service = projectService;
    }

    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException();

    private bool IsAdmin => User.IsInRole("ROLE_ADMIN");

    private static List<string> ParseTags(string? tags) =>
        (tags ?? "")
            .Split(',')
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrEmpty(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

    [HttpGet("/projects")]
    public async Task<IActionResult> Index()
    {
        var projects = await _service.GetByUserAsync(GetUserId());
        return View(projects);
    }

    [HttpGet("/Projects/Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("/Projects")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProject(ProjectCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View("Create", dto);

        var project = new Projects
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UserId = GetUserId()
        };

        await _service.CreateAsync(project, ParseTags(dto.Tags));
        TempData["Success"] = "Project created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/Projects/Edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        var project = await _service.GetByIdAsync(id);
        if (project == null)
            return NotFound();

        if (project.UserId != GetUserId() && !IsAdmin)
            return Forbid();

        var dto = new ProjectUpdateDto
        {
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Tags = string.Join(", ",
                project.ProjectTags?.Select(pt => pt.Tag.Name) ?? Enumerable.Empty<string>())
        };

        return View(dto);
    }

    [HttpPost("/Projects/Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ProjectUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var project = await _service.GetByIdAsync(id);
        if (project == null)
            return NotFound();

        if (project.UserId != GetUserId() && !IsAdmin)
            return Forbid();

        var updatedProject = new Projects
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UserId = project.UserId   // egasi o'zgarmaydi (admin tahrirlasa ham)
        };

        await _service.UpdateAsync(updatedProject, ParseTags(dto.Tags));
        TempData["Success"] = "Project updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/Projects/DeleteBulk")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBulk(List<string> ids)
    {
        if (ids != null && ids.Count > 0)
        {
            var count = await _service.DeleteManyAsync(ids, GetUserId(), IsAdmin);
            TempData["Success"] = $"{count} project(s) deleted.";
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpGet("/tags/search")]
    public async Task<IActionResult> SearchTags(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Json(new List<string>());

        return Json(await _service.SearchTagsAsync(q.Trim()));
    }
}