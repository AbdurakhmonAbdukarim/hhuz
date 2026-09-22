using hhuz.Dto;
using hhuz.Models;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Service;

public interface UserService
{
     Task<Users> Login(LoginDto dto);
     Task<Users> Register(RegisterDto dto);
}