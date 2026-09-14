using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto request)
    {
        // 1. Bazadan foydalanuvchi login/parolini tekshirish (soxta misol)
        if (request.Username != "admin" || request.Password != "password")
        {
            return Unauthorized("Login yoki parol xato!");
        }

        // 2. Token ichiga joylanadigan foydalanuvchi ma'lumotlari (Claims)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        // 3. Maxfiy kalit va Algoritmni belgilash
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. Token obyektini yasash (Amal qilish muddati: 1 soat)
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = jwt });
    }

    // Protection ostidagi endpoint
    [Authorize] // Faqat Token taqdim etgan foydalanuvchilar kira oladi!
    [HttpGet("protected-data")]
    public IActionResult GetProtectedData()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok($"Xush kelibsiz, {username}! Bu faqat autentifikatsiyadan o'tganlar uchun yopiq ma'lumot.");
    }
}

public record LoginDto(string Username, string Password);