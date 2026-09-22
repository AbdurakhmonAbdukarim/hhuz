namespace hhuz.Models;

public class CandidateAttributeValues
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Value { get; set; }


    //1.many side with Users table
    public string UserId{ get; set; }
    public Users User{ get; set; }
    
    //2.many to one connection with Attribute
    public string AttributeId { get; set; }
    public Attributes Attribute { get; set; }
    
    //3. one to one connection with Attribute Options
    public string? AttributeOptionId { get; set; }
    public AttributeOptions? AttributeOption { get; set; }
}