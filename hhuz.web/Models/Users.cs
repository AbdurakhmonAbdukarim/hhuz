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
    public string username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    
    [Column(TypeName = "varchar(50)")]
    public Roles Role { get; set; } = Roles.ROLE_CANDIDATE;
    
    //one to one connection with CV
    public Cvs?  Cv { get; set; }
}