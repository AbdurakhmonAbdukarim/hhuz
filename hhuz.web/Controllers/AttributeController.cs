using hhuz.Dto.Attribute;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Controllers;


[Authorize(Roles = "ROLE_RECRUITER,ROLE_ADMIN")]
public class AttributeController :Controller
{
    private readonly AttributeService _service;
    public AttributeController(AttributeService service)
    {
        _service = service;
    }
    
    
    [HttpGet("/attributes")]
    public async Task<IActionResult> Index(string? categoryId = null, string? search = null)
    {
        var attributes = await _service.GetAllAsync(categoryId, search);
        var categories = await _service.GetCategoriesAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.Search = search;

        return View(attributes);
    }
      
    
    [HttpGet("/Attributes/Create")]
    public async Task<IActionResult> Create()
    {
        var categories = await _service.GetCategoriesAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        ViewBag.AttributeTypes = GetAttributeTypes();

        return View();
    }

    [HttpPost("/Attributes/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _service.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.AttributeTypes = GetAttributeTypes();
            return View("~/Views/Attribute/Create.cshtml", dto);
        }

        var attribute = new Attributes
        {
            Name = dto.Name,
            Description = dto.Description,
            DataType = (AttributeDataType)dto.DataType,
            CategoryId = dto.CategoryId
        };

        var options = dto.Options ?? new List<string>();

        await _service.CreateAsync(attribute, options);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/Attributes/Edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        var attribute = await _service.GetByIdAsync(id);
        if (attribute == null)
            return NotFound();

        var categories = await _service.GetCategoriesAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", attribute.CategoryId);
        ViewBag.AttributeTypes = GetAttributeTypes();

        var dto = new AttributeUpdateDto
        {
            Name = attribute.Name,
            Description = attribute.Description,
            DataType = (int)attribute.DataType,
            CategoryId = attribute.CategoryId,
            Version = attribute.Version,
            Options = attribute.AttributeOptions?.Select(o => o.Value).ToList() ?? new List<string>()
        };

        return View("~/Views/Attribute/Edit.cshtml", dto);
    }

    [HttpPost("/Attributes/Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, AttributeUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _service.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
            ViewBag.AttributeTypes = GetAttributeTypes();
            return View("~/Views/Attribute/Edit.cshtml", dto);
        }

        var attribute = new Attributes
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            DataType = (AttributeDataType)dto.DataType,
            CategoryId = dto.CategoryId
        };

        var options = dto.Options ?? new List<string>();

        try
        {
            await _service.UpdateAsync(attribute, options, dto.Version);
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("", "This attribute was modified by another user. Please reload and try again.");
            var categories = await _service.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
            ViewBag.AttributeTypes = GetAttributeTypes();
            return View("~/Views/Attribute/Edit.cshtml", dto);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/Attributes/Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/Attributes/DeleteBulk")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBulk(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
            return RedirectToAction(nameof(Index));

        await _service.DeleteManyAsync(ids);

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> GetAttributeTypes()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "String (single-line)" },
            new SelectListItem { Value = "1", Text = "Text (Markdown)" },
            new SelectListItem { Value = "2", Text = "Image" },
            new SelectListItem { Value = "3", Text = "Numeric" },
            new SelectListItem { Value = "4", Text = "Date" },
            new SelectListItem { Value = "5", Text = "Period (date range)" },
            new SelectListItem { Value = "6", Text = "Boolean (checkbox)" },
            new SelectListItem { Value = "7", Text = "Dropdown (one of many)" }
        };
    }
}