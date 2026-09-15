using hhuz.Dto;
using hhuz.Mapper;
using hhuz.Models;
using hhuz.web.Data;
using Microsoft.AspNetCore.Identity;
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

    public Task<bool> Register(LoginDto dto)
    {
        bool exists = _context.Users.Any(u => u.Email == dto.email);
        if (!exists)
        {
            var Users = _userMapper.ToEntity(dto);
            _context.Add(Users);
            int row = _context.SaveChanges();
        }
        else
        {
            throw new Exception("Email already exists");
        }

        return Task.FromResult(true);
    }

    public Task<bool> LoginIn(LoginDto dto)
    {
        bool exists = _context.Users.Any(u => u.Email == dto.email);
//         if (exists)
//         {
//             return null;
//         }
// return null
//     
        return null;
    }
}