namespace hhuz.Models;

public class ExternalLogins
{
    public string Id { get; set; } = Guid.NewGuid().ToString();                                                 
    public string LoginProvider { get; set; }
    public string Name { get; set; }
    
    
    
    public string UserId { get; set; }
    public Users User { get; set; }
}