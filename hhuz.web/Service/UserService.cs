using hhuz.Dto;

namespace hhuz.Service;

public interface UserService
{
    Task<bool> Register(LoginDto dto);
    Task<bool> LoginIn(LoginDto dto);
    
}