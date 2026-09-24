using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Models;

[Index(nameof(Email), IsUnique = true)]
public class Users
{
    [Key]
    public string  Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Column("username")]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "varchar(50)")]
    public Roles Role { get; set; } 
    
    public bool IsBlocked { get; set; } = false;
    
    //1. one side connection with CV
    public ICollection<Cvs> Cvs { get; set; } = new List<Cvs>();
    
    //2.Profile one to one connection
    public Profiles?  Profile { get; set; }
    
    //3.one to many with Projects 
    public ICollection<Projects> Projects { get; set; } = new List<Projects>();
    
    //4candidate Attribute values
    public ICollection<CandidateAttributeValues> CandidateAttributeValues { get; set; } = new List<CandidateAttributeValues>();
    
    //5discussion post 
    public ICollection<DiscussionPosts>  DiscussionPosts { get; set; } = new List<DiscussionPosts>();
    
    //6Likes
    public ICollection<Likes> Likes { get; set; } = new List<Likes>();
    
    //7External login
    public ICollection<ExternalLogins>  ExternalLogins { get; set; } = new List<ExternalLogins>();
    
    
    

}