using System.Security.Claims;
using hhuz.Dto;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Controllers;

[Authorize]
public class CvsController : Controller
{
    private readonly CVService _cvService;
    private readonly ProjectService _projectService;
    private readonly CandidateProfileService _profileService;
    private readonly LikeService _likeService;

    public CvsController(
        CVService cvService,
        ProjectService projectService,
        CandidateProfileService profileService,
        LikeService likeService)
    {
        _cvService = cvService;
        _projectService = projectService;
        _profileService = profileService;
        _likeService = likeService;
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    [HttpGet("/cvs")]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var cvs = await _cvService.GetByUserAsync(userId);
        return View(cvs);
    }

    [HttpPost("/cvs/create/{positionId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string positionId)
    {
        var userId = GetUserId();

        try
        {
            var cv = await _cvService.CreateAsync(userId, positionId);
            return RedirectToAction(nameof(Edit), new { id = cv.Id });
        }
        catch (InvalidOperationException)
        {
            TempData["Message"] = "You already have a CV for this position.";
            return RedirectToAction("Index", "Positions");
        }
    }

    [HttpGet("/cvs/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        var cv = await _cvService.GetByIdAsync(id);
        if (cv == null)
            return NotFound();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isOwner = cv.UserId == userId;
        var isRecruiter = User.IsInRole("ROLE_RECRUITER") || User.IsInRole("ROLE_ADMIN");

        if (!isOwner && !isRecruiter)
            return Forbid();

        var attributeValues = await _profileService.GetAttributeValuesAsync(cv.UserId);

        var positionTagIds = cv.Position?.PositionProjectTags?
            .Select(pt => pt.TagId)
            .ToList() ?? new List<string>();

        var filteredProjects = (await _projectService.GetByUserAsync(cv.UserId))
            .Where(p => positionTagIds.Any() && 
                        p.ProjectTags?.Any(projTag => positionTagIds.Contains(projTag.TagId)) == true)
            .ToList();

        var viewModel = new CvViewViewModel
        {
            Cv = cv,
            Position = cv.Position!,
            Projects = filteredProjects,
            AttributeValues = attributeValues,
            IsOwner = isOwner,
            IsRecruiter = isRecruiter,
            LikeCount = await _likeService.GetCountAsync(cv.Id),
            HasLiked = userId != null && await _likeService.HasLikedAsync(cv.Id, userId)
        };

        return View(viewModel);
    }

    [HttpPost("/cvs/{id}/like")]
    [Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLike(string id)
    {
        await _likeService.ToggleAsync(id, GetUserId());
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("/cvs/{id}/edit")]
    public async Task<IActionResult> Edit(string id)
    {
        var cv = await _cvService.GetByIdAsync(id);
        if (cv == null)
            return NotFound();

        var userId = GetUserId();
        var isAdmin = User.IsInRole("ROLE_ADMIN");
        if (cv.UserId != userId && !isAdmin)
            return Forbid();

        if (cv.Status == CvStatus.PUBLISHED)
            return BadRequest("Cannot edit published CV");

        var attributeValues = await _profileService.GetAttributeValuesAsync(cv.UserId);

        var viewModel = new CvEditViewModel
        {
            CvId = cv.Id,
            PositionId = cv.PositionId,
            CvVersion = cv.Version,
            PositionTitle = cv.Position?.Title ?? "Unknown Position",
            PositionAttributes = cv.Position?.PositionAttributes?
                .OrderBy(pa => pa.SortOrder)
                .Select(pa => new PositionAttributeDto
                {
                    AttributeId = pa.AttributeId,
                    Name = pa.Name,
                    DataType = (int)pa.Attribute.DataType,
                    CurrentValue = attributeValues.FirstOrDefault(av => av.AttributeId == pa.AttributeId)?.Value,
                    CurrentOptionId = attributeValues.FirstOrDefault(av => av.AttributeId == pa.AttributeId)?.AttributeOptionId,
                    Options = pa.Attribute.AttributeOptions?
                        .Select(ao => new AttributeOptionDto { Id = ao.Id, Value = ao.Value })
                        .ToList() ?? new()
                })
                .ToList() ?? new()
        };

        return View(viewModel);
    }

    [HttpPost("/cvs/{id}/save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(string id, [FromForm] CvEditViewModel model)
    {
        var cv = await _cvService.GetByIdAsync(id);
        if (cv == null)
            return NotFound();

        var userId = GetUserId();
        var isAdmin = User.IsInRole("ROLE_ADMIN");
        if (cv.UserId != userId && !isAdmin)
            return Forbid();

        if (cv.Status == CvStatus.PUBLISHED)
            return BadRequest("Cannot edit published CV");

        try
        {
            await _cvService.CheckAndBumpVersionAsync(id, model.CvVersion);
        }
        catch (DbUpdateConcurrencyException)
        {
            TempData["Error"] = "This CV was modified elsewhere. Please reload and try again.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        await _profileService.SaveValuesAsync(cv.UserId, model.AttributeValues, model.AttributeOptions);

        TempData["Success"] = "CV saved successfully!";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost("/cvs/{id}/publish")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(string id)
    {
        try
        {
            await _cvService.PublishAsync(id, GetUserId());
            TempData["Success"] = "CV published successfully!";
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost("/cvs/{id}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var cv = await _cvService.GetByIdAsync(id);
        if (cv == null)
            return NotFound();

        var userId = GetUserId();
        if (cv.UserId != userId)
            return Forbid();

        await _cvService.DeleteAsync(id, userId);

        TempData["Success"] = "CV deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
    
    
    [HttpPost("/cvs/delete-selected")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSelected(List<string> ids)
    {
        var userId = GetUserId();
        if (ids.Count > 0)
            await _cvService.DeleteManyAsync(ids, userId);

        TempData["Success"] = "Selected CVs deleted.";
        return RedirectToAction(nameof(Index));
    }
}