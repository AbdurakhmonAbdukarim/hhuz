using System.Runtime.InteropServices.JavaScript;

namespace hhuz.Models;

public class Cvs
{
    public string Id{get;set;} =Guid.NewGuid().ToString();
    public string Status{get;set;}
    public string Version{get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //one to one connection  User
    public string UserId { get; set; }
    public Users user { get; set; } = null;
}