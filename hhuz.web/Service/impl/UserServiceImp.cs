using hhuz.Dto;
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
    private readonly PasswordHasher<Users>  _passwordHasher;
    
    public UserServiceImp(AppDbContext config)
    {
        _context = config;
        _passwordHasher = new PasswordHasher<Users>();
    }

    public async Task<Users> Register(RegisterDto dto)
    {
        if (dto.role == Roles.ROLE_ADMIN)
        {
            throw new  Exception("You cannot register a new user");
            
        }
        bool exists = await _context.Users.AnyAsync(u => u.Email == dto.email);
        Console.WriteLine(dto.role);
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

    public async Task<List<Users>> GetAllAsync(string? search = null)
    {
        var query = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Email.ToLower().Contains(search.ToLower()) ||
                u.Username.ToLower().Contains(search.ToLower()));

        return await query.OrderBy(u => u.Email).ToListAsync();
    }

    public async Task<Users?> GetByIdAsync(string id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new Exception("User not found");
    }

    public async Task SetRoleAsync(string userId, string role)
    {
        var usr = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId)
            ?? throw new Exception("User not found");
        usr.Role = (Roles)Enum.Parse(typeof(Roles), role);
        
        await _context.SaveChangesAsync();

    }

    public Task BlockAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task UnblockAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Users> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new Exception("User not found");

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            dto.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Password doesn't match");

        return user;
    }
}