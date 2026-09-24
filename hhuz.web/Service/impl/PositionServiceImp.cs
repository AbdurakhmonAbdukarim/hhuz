using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class PositionServiceImp :PositionService
{
    private readonly AppDbContext  _context;

    public PositionServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<Positions?> GetByIdAsync(string id)
    { 
        var pos= await _context.FindAsync<Positions>(id);
        if (pos == null)
            throw new Exception("Unable to find position with id: " + id);
        
        return pos;
    }

    public async Task<List<Positions>> GetAllAsync()
    {
        return  await _context.Positions
            .AsNoTracking().OrderByDescending(p=>p.CreatedAt).ToListAsync();
    }

    public async Task<Positions> CreateAsync(Positions position)
    {
        {
            _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return position;
        }
    }

    public async Task<Positions> UpdateAsync(Positions position, int expectedVersion)
    {
        var exs = await _context.Positions.FirstOrDefaultAsync(p => p.Id == position.Id);
        if (exs == null)
            throw new KeyNotFoundException($"Position not found: {position.Id}");

        if (exs.Version != expectedVersion)
            throw new DbUpdateConcurrencyException("Version mismatch");

        exs.Title = position.Title;
        exs.ShortDescription= position.ShortDescription;
        exs.MaxProjects= position.MaxProjects;

        exs.Version++;
        _context.Positions.Update(exs);
        await _context.SaveChangesAsync();
        
        return exs;

    }

    public async Task<bool> DeleteAsync(string id)
    {
        var pos = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        if (pos == null)
            return false;

        pos.IsDeleted = true;
        pos.Version++;

        _context.Positions.Update(pos);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<int> DeleteManyAsync(List<string> ids)
    {
        return await _context.Positions
            .Where(p => ids.Contains(p.Id) && !p.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true));
    }
}