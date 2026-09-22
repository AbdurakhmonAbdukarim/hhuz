using hhuz.Models;

namespace hhuz.Service;

public interface PositionService
{
    Task<Positions?> GetByIdAsync(string id);

    Task<List<Positions>> GetAllAsync();

    Task<Positions> CreateAsync(Positions position);

    Task<Positions> UpdateAsync(Positions position, int expectedVersion);

    Task<bool> DeleteAsync(string id);
}