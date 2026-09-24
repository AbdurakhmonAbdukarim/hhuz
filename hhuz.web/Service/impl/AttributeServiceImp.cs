using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class AttributeServiceImp : AttributeService
{
    private readonly AppDbContext  _context;
    public AttributeServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<List<Attributes>> GetAllAsync(string? categoryId = null, string? search = null)
    {
        var query = _context.Attributes
            .Include(a=>a.Category)
            .Include(a=>a.AttributeOptions)
            .AsNoTracking()
            .AsQueryable();    
        if (categoryId != null)
            query  = query.Where(a => a.CategoryId == categoryId);
        if (search != null)
            query = query.Where(a => a.Name.Contains(search));

        return query.OrderBy(a => a.Category.Name)
            .ThenBy(a => a.Name).ToList();
    }

    public async Task<Attributes?> GetByIdAsync(string id)
    {
        return await _context.Attributes
            .Include(a => a.Category)
            .Include(a => a.AttributeOptions)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Attributes> CreateAsync(Attributes attribute, List<string> options)
    {
        await _context.Attributes.AddAsync(attribute);

        foreach (var opt in options.Where(a=>!string.IsNullOrWhiteSpace(a)).Distinct())
        {
            _context.AttributeOptions.AddAsync(new AttributeOptions()
            {
                AttributeId = attribute.Id,
                Value = opt.Trim()
            });
        }
        await _context.SaveChangesAsync();
        return attribute;
    }

    public async Task<Attributes> UpdateAsync(Attributes attribute, List<string> options, int expectedVersion)
    {
        var usr = await _context.Attributes.
            Include(a => a.Category).
            Include(a => a.AttributeOptions).
            FirstOrDefaultAsync(a => a.Id == attribute.Id)
            ?? throw new KeyNotFoundException($"Attribute not found{attribute.Id}");

        if (usr.Version != expectedVersion)
            throw new DbUpdateConcurrencyException("Version mismatch");

        usr.Name = attribute.Name;
        usr.Description = attribute.Description;
        usr.CategoryId = attribute.CategoryId;
        usr.DataType  = attribute.DataType;
        usr.Version++;
        _context.AttributeOptions.RemoveRange(usr.AttributeOptions);

        foreach (var opt in options.Where(a=>!string.IsNullOrWhiteSpace(a)).Distinct())
        {
            usr.AttributeOptions.Add(new  AttributeOptions()
            {
                AttributeId = attribute.Id,
                Value = opt.Trim()
            });
        }
        await _context.SaveChangesAsync();
        return usr;

    }

    public async Task<bool> DeleteAsync(string id)
    {
        var attr = await _context.Attributes.FindAsync(id);
        if (attr == null) return false;

        _context.Attributes.Remove(attr);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteManyAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0) return;

        await _context.Attributes
            .Where(a => ids.Contains(a.Id))
            .ExecuteDeleteAsync();
    }

    public async Task<List<Categories>> GetCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}