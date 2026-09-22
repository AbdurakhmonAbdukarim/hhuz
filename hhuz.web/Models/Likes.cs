namespace hhuz.Models;

public class Likes
{
    public string Id { get; set; }= Guid.NewGuid().ToString();
    
    //many side connection with user
    public string UserId { get; set; }
    public Users User { get; set; }
    
    //Many side connection with CV
    public string CvId { get;set; }
    public Cvs Cv { get; set; }
}