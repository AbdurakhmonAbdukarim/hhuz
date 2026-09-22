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
    public async Task<IActionResult> Register(RegisterDto request)
{
    try
    {
        var users= await _userService.Register(request);
        SetToken( await _jwtService.GenerateJwtToken(users));
        return RedirectToAction("Index", "Home");
    }
    catch (Exception ex) when (ex.Message == "Email already exists")
    {
        TempData["Message"] = "Email already exists, sign in";

        return RedirectToAction("Login", "Auth");
    }
    catch (Exception ex) when (ex.Message == "Username already exists")
    {
        TempData["Message"] = "Username already exists";

        return RedirectToAction("Register", "Auth");
    }
    catch (Exception ex) when (ex.Message == "Invalid role")
    {
        TempData["Message"] = "Invalid role";

        return RedirectToAction("Register", "Auth");
    }
}

    [HttpPost("login")]
    public  async Task<IActionResult> Login(LoginDto request)
    {
        try
        {
            var exists = await _userService.Login(request);
            
            SetToken(await _jwtService.GenerateJwtToken(exists));
            return RedirectToAction("Index", "Home");
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
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("X-Access-Token");
        
        return RedirectToAction
            ("Index", "Home");
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



    