using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class CvServiceImp :CVService
{
    private readonly AppDbContext  _context;
    public CvServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Cvs>> GetByUserAsync(string userId)
    {
        return await _context.Cvs
            .Include(cv => cv.Position)
            .AsNoTracking()
            .Where(cv => cv.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Cvs>> GetByPositionAsync(string positionId)
    {
        return await _context.Cvs
            .Include(cvs => cvs.User)
            .AsNoTracking()
            .Where(cv => cv.PositionId == positionId)
            .ToListAsync();
    }

    public async Task<Cvs?> GetByIdAsync(string id)
    {
        return await _context.Cvs
            .Include(cv=>cv.Position)
            .ThenInclude(p=>p.PositionAttributes)
            .ThenInclude( pa=>pa.Attribute)
            .ThenInclude(o=>o.AttributeOptions)
            .Include(u=>u.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(cv => cv.Id == id);
    }

    public async Task<Cvs> CreateAsync(string userId, string positionId)
    {
        var exists = await _context.Cvs
            .AnyAsync(c => c.UserId == userId && c.PositionId == positionId);

        if (exists)
            throw new InvalidOperationException("CV already exists for this position.");

        var cv = new Cvs
        {
            UserId = userId,
            PositionId = positionId,
            Status = CvStatus.DRAFT
        };

        await _context.Cvs.AddAsync(cv);
        await _context.SaveChangesAsync();
        return cv;
    }

    public async Task PublishAsync(string cvId, string userId)
    {
        var cv = await _context.Cvs
                     .FirstOrDefaultAsync(c => c.Id == cvId && c.UserId == userId)
                 ?? throw new KeyNotFoundException("CV not found.");

        cv.Status = CvStatus.PUBLISHED;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string cvId, string userId)
    {
        var cv = await _context.Cvs
            .FirstOrDefaultAsync(c => c.Id == cvId && c.UserId == userId);

        if (cv == null) return false;

        _context.Cvs.Remove(cv);
        await _context.SaveChangesAsync();
        return true;
        
        
    }
    
    public async Task<int> DeleteManyAsync(List<string> ids, string userId)
    {
        return await _context.Cvs
            .Where(c => ids.Contains(c.Id) && c.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<int> CheckAndBumpVersionAsync(string cvId, int expectedVersion)
    {
        var cv = await _context.Cvs.FirstOrDefaultAsync(c => c.Id == cvId)
            ?? throw new KeyNotFoundException("CV not found.");

        if (cv.Version != expectedVersion)
            throw new DbUpdateConcurrencyException("Version mismatch");

        cv.Version++;
        await _context.SaveChangesAsync();
        return cv.Version;
    }
}