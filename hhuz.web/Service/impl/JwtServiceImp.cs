using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hhuz.Dto;
using hhuz.Models;
using hhuz.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace hhuz.Service;

public class JwtServiceImp : JwtService
{   
    private readonly IConfiguration _configuration;
    private readonly AppDbContext  _appDbContext;
    public JwtServiceImp(IConfiguration configuration, AppDbContext appDbContext)
    {
        _configuration = configuration;
        _appDbContext = appDbContext;
        
    }
    
    public async Task<string> GenerateJwtToken(LoginDto dto)
    {
        Users? user =await _appDbContext.Users.FirstOrDefaultAsync(d => d.Email == dto.email) ;
        if (user == null)
        {
            throw new Exception("User not found"); 
        }
        var jwtSettings = _configuration.GetSection("jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "");

        var clams = new[]
        {
            new Claim(ClaimTypes.Name, user.username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }
        ;
        var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: clams,
                    expires: DateTime.UtcNow.AddMinutes(1),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256
                    )
                );
        return  new JwtSecurityTokenHandler().WriteToken(token);
    }
}