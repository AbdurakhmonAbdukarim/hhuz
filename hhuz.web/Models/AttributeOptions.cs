namespace hhuz.Models;

public class AttributeOptions
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Value { get; set; }
    
    //1.Many side connection with Attribute
    public string AttributeId { get; set; }
    public Attributes Attribute { get; set; }
}