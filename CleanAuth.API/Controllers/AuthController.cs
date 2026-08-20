using CleanAuth.Application.DTOs;
using CleanAuth.Application.DTOs.Auth;
using CleanAuth.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;

    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Message);
        }

        return Ok(result.Data);
    }
    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            UserId = userId,
            Email = email
        });
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Message);
        }

        return Ok(result.Data);
    }
}