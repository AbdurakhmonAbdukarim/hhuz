namespace hhuz.Models;

public class Attributes
{
    public  string Id { get; set; }= Guid.NewGuid().ToString();
    
    public string Name { get; set; }
    public string Description { get; set; }
    public AttributeDataType DataType { get; set; }

    
    //1.many connection with Category
    public string CategoryId { get; set; }
    public Categories Category { get; set; }
    
    //2.one connection side to Options 
    public ICollection<AttributeOptions>  AttributeOptions { get; set; } = new List<AttributeOptions>(); 
    
    //4.one side connection with CandidateAttributeValues
    public ICollection<CandidateAttributeValues> CandidateAttributeValues { get; set; }
        = new List<CandidateAttributeValues>();

    //4.one side connection with PositionAttributes
    public ICollection<PositionAttributes> PositionAttributes { get; set; }
        = new List<PositionAttributes>();
}
    
    