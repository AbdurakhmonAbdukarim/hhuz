using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hhuz.Dto;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly UserService  _userService;
    
    public AuthController(IConfiguration config,UserService  userService)
    {
        _config = config;
        _userService = userService;
        
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View("~/Views/Auth/Register.cshtml");
    }

    [HttpPost("register")]
    public IActionResult Register(LoginDto request)
    {
        Console.WriteLine(request.email);
        return View("~/Views/Auth/Register.cshtml", request);
    }
}

    