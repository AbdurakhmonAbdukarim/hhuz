namespace hhuz.Dto;

public class DiscussionPostDto
{
    public string Id { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsAuthor { get; set; }
}