namespace hhuz.Dto;

public class ProjectUpdateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Tags { get; set; }
}