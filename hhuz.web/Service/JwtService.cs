namespace hhuz.Service;

public interface JwtService
{
    string GenerateJwtToken(string username, string email);
}