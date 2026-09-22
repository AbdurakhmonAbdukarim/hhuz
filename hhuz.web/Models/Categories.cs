namespace hhuz.Models;

public class Categories
{
    public  string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; }
    public string Description { get; set; }
    
    //1. one side connection with Attributes
    public ICollection<Attributes>  Attributes { get; set; } = new List<Attributes>();
    
}