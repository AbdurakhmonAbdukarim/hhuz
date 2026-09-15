using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace hhuz.Models;

[Index(nameof(Email), IsUnique = true)] // Email unique bo'ladi
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
}