namespace hhuz.Models;

public class ProjectTags
{
    public string Id{get;set;} = Guid.NewGuid().ToString();
    
    //1. many side connection with Project
    public string ProjectId{get;set;}
    public Projects Project { get; set; } = null;
    
    //2.many side connection with Tag
    public string TagId{get;set;}
    public Tags Tag { get; set; } = null;
}