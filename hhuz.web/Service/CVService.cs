using hhuz.Models;

namespace hhuz.Service;

public interface CVService
{
    Task<List<Cvs>> GetByUserAsync(string userId);
    Task<List<Cvs>> GetByPositionAsync(string positionId);
    Task<Cvs?> GetByIdAsync(string id);
    Task<Cvs> CreateAsync(string userId, string positionId);
    Task PublishAsync(string cvId, string userId);
    Task<bool> DeleteAsync(string cvId, string userId);

    Task<int> DeleteManyAsync(List<string> ids, string userId);
    Task<int> CheckAndBumpVersionAsync(string cvId, int expectedVersion);
}