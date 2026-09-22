using hhuz.Dto;
using hhuz.Mapper;
using hhuz.Models;
using hhuz.web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace hhuz.Service;

public class UserServiceImp : UserService
{
    public readonly AppDbContext _context;
    private readonly BaseMapper<Users, LoginDto> _userMapper;
    private readonly PasswordHasher<Users>  _passwordHasher;
    
    public UserServiceImp(AppDbContext config)
    {
        _context = config;
        _passwordHasher = new PasswordHasher<Users>();
    }

    public async Task<Users> Register(RegisterDto dto)
    {
        if (dto.role == Roles.ROLE_CANDIDATE && dto.role == Roles.ROLE_RECRUITOR)
            throw new  Exception("You cannot register a new user");
        {
            
        }
        bool exists = await _context.Users.AnyAsync(u => u.Email == dto.email);
        
        if (!exists) {

            var user = new Users()
            {
                Username = dto.username,
                Email = dto.email,
                Role = dto.role

            };
            user.Password = _passwordHasher.HashPassword(user, dto.password);
            
            _context.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        else
        {
            throw new Exception("Email already exists");
        }
        
    }

    public async Task<Users> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.email);

        if (user == null)
            throw new Exception("User not found");

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            dto.password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Password doesn't match");

        return user;
    }
}