using hhuz.Dto;

namespace hhuz.Service;

public interface JwtService
{
    Task<string> GenerateJwtToken(LoginDto dto);
}