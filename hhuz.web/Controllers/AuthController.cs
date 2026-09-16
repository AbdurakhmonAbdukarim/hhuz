using System.Linq.Expressions;
using hhuz.Dto;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [AllowAnonymous]
    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        return View("~/Views/Auth/Register.cshtml");
    }
    [AllowAnonymous]
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
        
        string token = await _jwtService.GenerateJwtToken(request);
        SetToken(token);
        return RedirectToAction("Index", "Home");
    }
    catch (Exception ex) when (ex.Message == "Email already exists")
    {
        TempData["Message"] = "Email already exists, sign in";
        
        return RedirectToAction("Login", "Auth");
    }
}

    [HttpPost("login")]
    public  async Task<IActionResult> Login(LoginDto request)
    {
        var exists = await _userService.Login(request);
        try
        {
            string token =await _jwtService.GenerateJwtToken(request);
            SetToken(token);
        }
        catch (Exception ex) when (ex.Message == "Password doesn't match")
        {
            TempData["Message"] = "Password doesn't match";
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex) when (ex.Message == "User not found")
        {
            TempData["Message"] = "User not found";
            return RedirectToAction("Register", "Auth");
        }
        return RedirectToAction("Index", "Home");
        
    }
    
    private void SetToken(string token)
    {
        Response.Cookies.Append("X-Access-Token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(2)
        });
    }
}



    