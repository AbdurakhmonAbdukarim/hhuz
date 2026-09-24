using hhuz.Dto;
using hhuz.Models;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Service;

public interface UserService
{
     Task<Users> Login(LoginDto dto);
     Task<Users> Register(RegisterDto dto);
     Task<List<Users>> GetAllAsync(string? search = null);
     Task<Users?> GetByIdAsync(string id);
     Task SetRoleAsync(string userId, string role);
     Task BlockAsync(string userId);
     Task UnblockAsync(string userId);
     Task DeleteAsync(string userId);
}