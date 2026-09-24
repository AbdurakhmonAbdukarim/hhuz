using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Service;

public class LikeServiceImp : LikeService
{
    private readonly AppDbContext _context;

    public LikeServiceImp(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<int> GetCountAsync(string cvId)
    {
        return await _context.Likes.CountAsync(cv => cv.CvId == cvId);
    }

    public async Task<bool> HasLikedAsync(string cvId, string userId)
    {
        return await _context.Likes.AnyAsync(a => a.CvId==cvId && a.UserId == userId);
    }

    public async Task ToggleAsync(string cvId, string userId)
    {
        var existing = await _context.Likes
            .FirstOrDefaultAsync(l => l.CvId == cvId && l.UserId == userId);

        if (existing != null)
        {
            _context.Likes.Remove(existing);
        }
        else
        {
            await _context.Likes.AddAsync(new Likes
            {
                CvId = cvId,
                UserId = userId
            });
        }

        await _context.SaveChangesAsync();
    }
}
