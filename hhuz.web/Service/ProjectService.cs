using hhuz.Models;

namespace hhuz.Service;

public interface ProjectService
{
    Task<List<Projects>> GetByUserAsync(string userId);
    Task<Projects?> GetByIdAsync(string id);
    Task<Projects> CreateAsync(Projects project, List<string> tagNames);
    Task<Projects> UpdateAsync(Projects project, List<string> tagNames);
    Task<bool> DeleteAsync(string id);
    Task<int> DeleteManyAsync(List<string> ids, string userId, bool isAdmin);
    Task<List<string>> SearchTagsAsync(string prefix, int limit = 10);

}