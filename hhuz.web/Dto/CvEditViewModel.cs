// Dto/CvEditViewModel.cs
namespace hhuz.Dto;

public class CvEditViewModel
{
    public string CvId { get; set; } = null!;
    public string PositionId { get; set; } = null!;
    public int CvVersion { get; set; }
    
    // Attribute values — form'dan keladi
    public Dictionary<string, string> AttributeValues { get; set; } = new();
    public Dictionary<string, string> AttributeOptions { get; set; } = new();
    
    // Display uchun
    public string PositionTitle { get; set; } = null!;
    public List<PositionAttributeDto> PositionAttributes { get; set; } = new();
}

public class PositionAttributeDto
{
    public string AttributeId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DataType { get; set; }
    public string? CurrentValue { get; set; }
    public string? CurrentOptionId { get; set; }
    public List<AttributeOptionDto> Options { get; set; } = new();
}

public class AttributeOptionDto
{
    public string Id { get; set; } = null!;
    public string Value { get; set; } = null!;
}