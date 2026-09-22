using hhuz.Models;

namespace hhuz.Dto;

public class RegisterDto
{
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public Roles role { get; set; }
}