using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class CandidateProfileServiceImp : CandidateProfileService
{
    private readonly AppDbContext _context;

    public CandidateProfileServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<Profiles?> GetProfileAsync(string userId)
    {
        return await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Profiles> CreateOrUpdateProfileAsync(Profiles profile, int expectedVersion)
    {
        var existing = await _context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

        if (existing == null)
        {
            profile.Version = 1;
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        if (existing.Version != expectedVersion)
            throw new DbUpdateConcurrencyException("Version mismatch");

        existing.FirstName = profile.FirstName;
        existing.LastName = profile.LastName;
        existing.Location = profile.Location;
        existing.PhotoUrl = profile.PhotoUrl;
        existing.Version++;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<List<CandidateAttributeValues>> GetAttributeValuesAsync(string userId)
    {
        return await _context.CandidateAttributeValues
            .Include(v => v.Attribute)
            .ThenInclude(a => a.Category)
            .Include(v => v.AttributeOption)
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .ToListAsync();
    }

    public async Task UpsertAttributeValueAsync(string userId, string attributeId, string? value, string? optionId)
    {
        var existing = await _context.CandidateAttributeValues
            .FirstOrDefaultAsync(v => v.UserId == userId && v.AttributeId == attributeId);

        if (existing == null)
        {
            await _context.CandidateAttributeValues.AddAsync(new CandidateAttributeValues
            {
                UserId = userId,
                AttributeId = attributeId,
                Value = value,
                AttributeOptionId = optionId
            });
        }
        else
        {
            existing.Value = value;
            existing.AttributeOptionId = optionId;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAttributeValueAsync(string userId, string attributeId)
    {
        var existing = await _context.CandidateAttributeValues
            .FirstOrDefaultAsync(v => v.UserId == userId && v.AttributeId == attributeId);

        if (existing != null)
        {
            _context.CandidateAttributeValues.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task SaveValuesAsync(string userId,
        Dictionary<string, string> values, Dictionary<string, string> options)
    {
        var attributeIds = values.Keys.Union(options.Keys).ToList();
        if (attributeIds.Count == 0) return;

        var existing = await _context.CandidateAttributeValues
            .Where(v => v.UserId == userId && attributeIds.Contains(v.AttributeId))
            .ToDictionaryAsync(v => v.AttributeId);

        foreach (var attrId in attributeIds)
        {
            if (!existing.TryGetValue(attrId, out var row))
            {
                row = new CandidateAttributeValues { UserId = userId, AttributeId = attrId };
                _context.CandidateAttributeValues.Add(row);
            }

            if (options.TryGetValue(attrId, out var optionId))
            {
                row.AttributeOptionId = string.IsNullOrEmpty(optionId) ? null : optionId;
                row.Value = null;
            }
            else
            {
                var value = values[attrId];
                row.Value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        await _context.SaveChangesAsync();
    }
}