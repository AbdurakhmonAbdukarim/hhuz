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

    public UserServiceImp(AppDbContext config, BaseMapper<Users, LoginDto> userMapper)
    {
        _context = config;
        _userMapper = userMapper;
    }

    public async Task<bool> Register(LoginDto dto)
    {
        bool exists = await _context.Users.AnyAsync(u => u.Email == dto.email);
        
        if (!exists)
        {
            var Users = _userMapper.ToEntity(dto);
            _context.Add(Users);
            int row = await _context.SaveChangesAsync();
            return row > 0;
        }
        else
        {
            throw new Exception("Email already exists");
        }
        
    }

    public async Task<bool> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.email);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.Password != dto.password)
        {
            throw new Exception("Passwords do not match"); 
        }

        return true;
    }
}