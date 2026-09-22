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
        List<Positions>? list = await _context.Positions.ToListAsync();
        return list;
    }

    public Task<Positions> CreateAsync(Positions position)
    {
        Positions pos = new Positions()
        {
            
        }
    }

    public Task<Positions> UpdateAsync(Positions position, int expectedVersion)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}