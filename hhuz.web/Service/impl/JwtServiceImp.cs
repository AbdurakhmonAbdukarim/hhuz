using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace hhuz.Service;

public class JwtServiceImp : JwtService
{   
    private readonly IConfiguration _configuration;
    public JwtServiceImp(IConfiguration configuration)
    {
        _configuration = configuration;
        
    }
    
    public string GenerateJwtToken(string username, string email)
    {
        var jwtSettings = _configuration.GetSection("jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "");

        var clams = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }
        ;
        var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: clams,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256
                    )
                );
        return  new JwtSecurityTokenHandler().WriteToken(token);
    }
}