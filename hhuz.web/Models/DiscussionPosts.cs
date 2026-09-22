namespace hhuz.Models;

public class DiscussionPosts
{
    public string Id { get; set; }= Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //many side connection with users
    public string UserId { get; set; }
    public Users User { get; set; }
    
    // many side connection with Position
    public string PositionId { get; set; }
    public Positions Position { get; set; }
}