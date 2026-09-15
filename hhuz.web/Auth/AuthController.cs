using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hhuz.Dto;
using hhuz.Models;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly UserService  _userService;
    private readonly JwtService  _jwtService;
    
    public AuthController(IConfiguration config,JwtService jwtService, UserService  userService)
    {
        _config = config;
        _userService = userService;
        _jwtService = jwtService;
        
    }

    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        return View("~/Views/Auth/Register.cshtml");
    }
    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        return View("~/Views/Auth/Login.cshtml");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginDto request)
{
    try
    {
        await _userService.Register(request);
        
        string token = _jwtService.GenerateJwtToken(request.username, request.email);
        Response.Cookies.Append("X-Access-Token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(2)
        });
        
        return RedirectToAction("Login", "Auth");
    }
    catch (Exception ex) when (ex.Message == "Email already exists")
    {
        TempData["Message"] = "Email allaqachon mavjud, tizimga kiring!";
        return RedirectToAction("Login", "Auth");
    }
}

    [HttpPost("login")]
    public Task<IActionResult> Login(LoginDto request)
    {
        _userService.LoginIn(request);
        return  Task.FromResult<IActionResult>(View("~/Views/Auth/Login.cshtml"));
    }
    
}



    