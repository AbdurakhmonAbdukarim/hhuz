using hhuz.Models;

namespace hhuz.Service;

public interface AttributeService
{
    Task<List<Attributes>> GetAllAsync(string? categoryId = null, string? search = null);
    Task<Attributes?> GetByIdAsync(string id);
    Task<Attributes> CreateAsync(Attributes attribute, List<string> options);
    Task<Attributes> UpdateAsync(Attributes attribute, List<string> options, int expectedVersion);
    Task<bool> DeleteAsync(string id);
    Task DeleteManyAsync(List<string> ids);
    Task<List<Categories>> GetCategoriesAsync();
}