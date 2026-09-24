using hhuz.Models;

namespace hhuz.Service;

public interface CandidateProfileService
{
    Task<Profiles?> GetProfileAsync(string userId);
    Task<Profiles> CreateOrUpdateProfileAsync(Profiles profile, int expectedVersion);

    Task<List<CandidateAttributeValues>> GetAttributeValuesAsync(string userId);
    Task UpsertAttributeValueAsync(string userId, string attributeId, string? value, string? optionId);
    Task RemoveAttributeValueAsync(string uerId, string attributeId); 
    Task SaveValuesAsync(string userId, Dictionary<string, string> values, Dictionary<string, string> options);

}