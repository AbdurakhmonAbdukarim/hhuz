using hhuz.Dto;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Controllers;

public class ProfileController :Controller
{
    private readonly CandidateProfileService _profileService;
    private readonly AttributeService _attributeService;
    private readonly ProjectService _projectService;
    private readonly CVService _cvService;

    public ProfileController(
        CandidateProfileService profileService,
        AttributeService attributeService,
        ProjectService projectService,
        CVService cvService)
    
    {
        _profileService = profileService;
        _attributeService = attributeService;
        _projectService = projectService;
        _cvService = cvService;
    }
    
    private string GetUserId()
    {
        return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
               ?? throw new UnauthorizedAccessException();
    }
    
    [HttpGet("/profile")]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();

        var profile = await _profileService.GetProfileAsync(userId);
        var attributeValues = await _profileService.GetAttributeValuesAsync(userId);
        var projects = await _projectService.GetByUserAsync(userId);
        var cvs = await _cvService.GetByUserAsync(userId);
        var allAttributes = await _attributeService.GetAllAsync();

        var viewModel = new ProfilePageViewModel
        {
            Profile = profile,
            AttributeValues = attributeValues,
            Projects = projects,
            Cvs = cvs,
            AvailableAttributes = allAttributes.Where(a => 
                !attributeValues.Any(av => av.AttributeId == a.Id)).ToList()
        };

        return View(viewModel);
        
    }
    
    [HttpPost("/profile/me")]
    public async Task<IActionResult> UpdateMe(ProfileMeDto dto)
    {
        var userId = GetUserId();

        var profile = new Profiles
        {
            UserId = userId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Location = dto.Location,
            PhotoUrl = dto.PhotoUrl
        };

        try
        {
            await _profileService.CreateOrUpdateProfileAsync(profile, dto.Version);
        }
        catch (DbUpdateConcurrencyException)
        {
            TempData["Error"] = "Your profile was modified elsewhere. Please reload and try again.";
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("/profile/attribute")]
    public async Task<IActionResult> SaveAttributeValue(
        [FromBody] AttributeValueDto dto)
    {
        var userId = GetUserId();

        // Validate attribute exists
        var attr = await _attributeService.GetByIdAsync(dto.AttributeId);
        if (attr == null)
            return NotFound();

        await _profileService.UpsertAttributeValueAsync(
            userId,
            dto.AttributeId,
            dto.Value,
            dto.OptionId);

        return Ok(new { success = true });
    }
    
    [HttpDelete("/profile/attribute/{attributeId}")]
    public async Task<IActionResult> RemoveAttributeValue(string attributeId)
    {
        var userId = GetUserId();

        await _profileService.RemoveAttributeValueAsync(userId, attributeId);

        return Ok(new { success = true });
    }

    [HttpPost("/profile/add-attribute/{attributeId}")]
    public async Task<IActionResult> AddAttribute(string attributeId)
    {
        var userId = GetUserId();

        var attr = await _attributeService.GetByIdAsync(attributeId);
        if (attr == null)
            return NotFound();

        // Empty value qo'shish
        await _profileService.UpsertAttributeValueAsync(userId, attributeId, null, null);

        return RedirectToAction(nameof(Index));
    }

}

