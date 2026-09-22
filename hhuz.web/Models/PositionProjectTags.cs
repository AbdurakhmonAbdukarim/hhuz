namespace hhuz.Models;

public class PositionProjectTags
{
    public string Id{get;set;} = Guid.NewGuid().ToString();
    
    //1.Many side connection with Project
    public string PositionId { get; set; }
    public Positions Position { get; set; }
    
    //2.Many side connection with Tags
    public string TagId { get; set; }
    public Tags Tag { get; set; }
    
}