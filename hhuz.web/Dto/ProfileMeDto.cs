namespace hhuz.Dto;

public class ProfileMeDto
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Location { get; set; } = "";
    public string? PhotoUrl { get; set; }
    public int Version { get; set; }
}