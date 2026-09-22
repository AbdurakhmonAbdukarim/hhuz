namespace hhuz.Models;

public class Positions

{
    public string Id{get;set;}= Guid.NewGuid().ToString();
    public string Title{get;set;}
    public string ShortDescription{get;set;}
    public int MaxProjects { get; set; }
    public int Version { get; set; } = 1;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //1. one side connection with AccessRules
    public ICollection<AccessRules>  AccessRules { get; set; } = new List<AccessRules>();
    
    //2.one side connection with DiscussionPosts
    public ICollection<DiscussionPosts>  DiscussionPosts { get; set; } = new List<DiscussionPosts>();
    
    //3.one side connection with PositionAttributes
    public ICollection<PositionAttributes>  PositionAttributes { get; set; } = new List<PositionAttributes>();
    
    //4. one side connection with PositionProjectTags
    public ICollection<PositionProjectTags>  PositionProjectTags { get; set; } = new List<PositionProjectTags>();
    
    //5. one side connection with Cv
    public ICollection<Cvs>  Cvs { get; set; } = new List<Cvs>();
    
    
    
    
}