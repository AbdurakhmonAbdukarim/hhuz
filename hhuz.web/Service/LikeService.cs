namespace hhuz.Service;

public interface LikeService
{
    Task<int> GetCountAsync(string cvId);
    Task<bool> HasLikedAsync(string cvId, string userId);
    Task ToggleAsync(string cvId, string userId);

}   