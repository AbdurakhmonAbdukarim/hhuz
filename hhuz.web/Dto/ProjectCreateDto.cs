namespace hhuz.Dto;

public class ProjectCreateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public string? Tags { get; set; } 
}