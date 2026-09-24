using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class DiscussionServiceImp : DiscussionService
{
    private readonly AppDbContext _context;
    public DiscussionServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<List<DiscussionPosts>> GetByPositionAsync(string positionId)
    {
        return await _context.DiscussionPosts
            .Include(u => u.User)
            .AsNoTracking()
            .Where(u => u.PositionId == positionId)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<DiscussionPosts> CreatePostAsync(string positionId, string userId, string content)
    {
        var post = new DiscussionPosts()
        {
            PositionId = positionId,
            UserId = userId,
            Content = content,
            Title = ""
        };
        _context.DiscussionPosts.AddAsync(post);
        await  _context.SaveChangesAsync();
        
        return post;

    }

    public async Task<bool> DeletePostAsync(string postId, string userId)
    {
        var post = await _context.DiscussionPosts
            .FirstOrDefaultAsync(dp => dp.Id == postId);

        if (post == null)
            return false;

        // Only author or admin can delete
        if (post.UserId != userId && !await _context.Users
                .AnyAsync(u => u.Id == userId && (u.Role == Roles.ROLE_ADMIN)))
            throw new UnauthorizedAccessException();

        _context.DiscussionPosts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }
}