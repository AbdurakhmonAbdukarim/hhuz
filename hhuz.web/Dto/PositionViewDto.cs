using hhuz.Models;

namespace hhuz.Dto;

public class PositionViewDto
{
    public Positions Position { get; set; } = null!;
    public List<DiscussionPostDto> Discussions { get; set; } = new();
}