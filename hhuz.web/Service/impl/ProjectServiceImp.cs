using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class ProjectServiceImp : ProjectService
{
    private readonly AppDbContext _context;

    public ProjectServiceImp(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Projects>> GetByUserAsync(string userId)
    {
        return await _context.Projects
            .AsNoTracking()
            .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    public async Task<Projects?> GetByIdAsync(string id)
    {
        return await _context.Projects
            .AsNoTracking()
            .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Projects> CreateAsync(Projects project, List<string> tagNames)
    {
        NormalizeDates(project);

        var tags = await GetOrCreateTagsAsync(tagNames);

        project.ProjectTags = tags
            .Select(t => new ProjectTags { Project = project, Tag = t })
            .ToList();

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();   // bitta tranzaksiya: yangi teglar + loyiha + bog'lanishlar

        return project;
    }

    public async Task<Projects> UpdateAsync(Projects project, List<string> tagNames)
    {
        var existing = await _context.Projects
            .Include(p => p.ProjectTags)
            .FirstOrDefaultAsync(p => p.Id == project.Id)
            ?? throw new KeyNotFoundException();

        NormalizeDates(project);

        existing.Name = project.Name;
        existing.Description = project.Description;
        existing.StartDate = project.StartDate;
        existing.EndDate = project.EndDate;

        var tags = await GetOrCreateTagsAsync(tagNames);
        var newTagIds = tags.Select(t => t.Id).ToHashSet();

        // Endi kerak bo'lmagan teglarni olib tashlaymiz
        var toRemove = existing.ProjectTags
            .Where(pt => !newTagIds.Contains(pt.TagId))
            .ToList();
        _context.ProjectTags.RemoveRange(toRemove);

        // Yangi qo'shilgan teglarni bog'laymiz (bu sikl bazaga so'rov yubormaydi)
        var currentTagIds = existing.ProjectTags.Select(pt => pt.TagId).ToHashSet();
        foreach (var tag in tags.Where(t => !currentTagIds.Contains(t.Id)))
        {
            existing.ProjectTags.Add(new ProjectTags { ProjectId = existing.Id, Tag = tag });
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _context.Projects
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }

    public async Task<int> DeleteManyAsync(List<string> ids, string userId, bool isAdmin)
    {
        var query = _context.Projects.Where(p => ids.Contains(p.Id));

        if (!isAdmin)
            query = query.Where(p => p.UserId == userId);

        return await query.ExecuteDeleteAsync();
    }

    public async Task<List<string>> SearchTagsAsync(string prefix, int limit = 10)
    {
        return await _context.Tags
            .Where(t => EF.Functions.ILike(t.Name, prefix + "%"))
            .OrderBy(t => t.Name)
            .Select(t => t.Name)
            .Take(limit)
            .ToListAsync();
    }

    private async Task<List<Tags>> GetOrCreateTagsAsync(List<string> tagNames)
    {
        var names = tagNames
            .Select(n => n.Trim())
            .Where(n => n.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (names.Count == 0)
            return new List<Tags>();

        var lowerNames = names.Select(n => n.ToLower()).ToList();

        var existing = await _context.Tags
            .Where(t => lowerNames.Contains(t.Name.ToLower()))
            .ToListAsync();

        var newTags = names
            .Where(n => !existing.Any(e => string.Equals(e.Name, n, StringComparison.OrdinalIgnoreCase)))
            .Select(n => new Tags { Name = n })
            .ToList();

        _context.Tags.AddRange(newTags);

        return existing.Concat(newTags).ToList();
    }

    private static void NormalizeDates(Projects project)
    {
        project.StartDate = DateTime.SpecifyKind(project.StartDate.Date, DateTimeKind.Utc);

        if (project.EndDate.HasValue)
            project.EndDate = DateTime.SpecifyKind(project.EndDate.Value.Date, DateTimeKind.Utc);
    }
}