using hhuz.Dto;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Service;

public interface UserService
{
     Task<bool> Login(LoginDto dto);
     Task<bool> Register(LoginDto dto);
}