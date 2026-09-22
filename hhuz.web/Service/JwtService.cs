using hhuz.Dto;
using hhuz.Models;

namespace hhuz.Service;

public interface JwtService
{
    Task<string> GenerateJwtToken(Users dto);
}