namespace hhuz.Models;

public class Tags
{
    public string  Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    
    //1. one side connection with PositionProjectTags
    public ICollection<PositionProjectTags> PositionProjectTags  { get; set; }  = new List<PositionProjectTags>();
    
    //2. one side connection with ProjectTag
    public ICollection<ProjectTags> ProjectTags { get; set; }  = new List<ProjectTags>();
    
}