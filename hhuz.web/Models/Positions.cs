namespace hhuz.Models;

public class Positions

{
    public string Id{get;set;}= Guid.NewGuid().ToString();
    public string Title{get;set;}
    public string ShortDescription{get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}