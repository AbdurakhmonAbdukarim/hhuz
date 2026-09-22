namespace hhuz.Models;

public class Profiles
{
    public string Id{get;set;}= Guid.NewGuid().ToString();
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Location { get; set; }

    public string? PhotoUrl { get; set; }

    
    //1.One to one join Users
    public string UserId{get;set;}
    public Users User { get; set; } = null;
    
    
    
    
}