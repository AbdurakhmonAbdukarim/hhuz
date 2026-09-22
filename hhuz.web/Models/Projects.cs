namespace hhuz.Models;

public class Projects
{
    public string Id{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    //one to one with Users
    public string UserId { get; set; } = string.Empty;
    public Users User { get; set; } = null;
    
    //one side connection with Tags
    public ICollection<ProjectTags> ProjectTags { get; set; } = new List<ProjectTags>();
    
}