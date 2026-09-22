using System.Runtime.InteropServices.JavaScript;

namespace hhuz.Models;

public class Cvs
{
    public string Id{get;set;} =Guid.NewGuid().ToString();
    public CvStatus Status { get; set; } = CvStatus.DRAFT;
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //1.many side connection with Users
    public string UserId { get; set; }
    public Users User { get; set; } = null;
    
    //2. many side connectoin with Position
    public string PositionId { get; set; }
    public Positions Position { get; set; }
    
    //3. one side connection with likes
    public ICollection<Likes>  Like { get; set; } = new List<Likes>();
}