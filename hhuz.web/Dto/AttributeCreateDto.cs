namespace hhuz.Dto.Attribute;

public class AttributeCreateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int DataType { get; set; }
    public string CategoryId { get; set; } = "";
    public List<string>? Options { get; set; }
}