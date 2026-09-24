using hhuz.Models;

namespace hhuz.Service;

public interface DiscussionService
{
    Task<List<DiscussionPosts>> GetByPositionAsync(string positionId);
    Task<DiscussionPosts> CreatePostAsync(string positionId, string userId, string content);
    Task<bool> DeletePostAsync(string postId, string userId);
}