using hhuz.Dto;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Service;

public interface UserService
{
     Task<bool> LoginIn(LoginDto dto);
     Task<bool> Register(LoginDto dto);
}