namespace hhuz.Models;

public class PositionAttributes
{
    public string  Id { get; set; }  = Guid.NewGuid().ToString();
    public string Name { get; set; }
    
    public string Description { get; set; }

    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }

    //1.many side connection with Position
    public string PositionId { get; set; }
    public Positions Position { get; set; }


    //2.many side connection with Attribute
    public string AttributeId { get; set; }
    public Attributes Attribute { get; set; }
}