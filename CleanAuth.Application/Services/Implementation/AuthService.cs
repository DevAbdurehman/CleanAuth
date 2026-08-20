
using CleanAuth.Application.Common;
using CleanAuth.Application.DTOs;
using CleanAuth.Application.DTOs.Auth;
using CleanAuth.Application.Interfaces.Repositories;
using CleanAuth.Application.Services.Interfaces;
using CleanAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanAuth.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;


    public AuthService(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }



    public async Task<Result> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userRepository.GetEmailAsync(request.Email);

        if (existingUser != null)
        {
            return Result.Failure("Email already exists.");
        }
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(
    new User(),
    request.Password
),
            IsEmailVerified = false,
            IsActive = true
        };


        await _userRepository.AddAsync(user);

        await _userRepository.SaveChangesAsync();

        return Result.Success("User registered successfully.");
    }
    public async Task<Result> LoginAsync(LoginDto request)
    {
        var user = await _userRepository.GetEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Failure("Invalid email or password.");
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Result.Failure("Invalid email or password.");
        }

        var token = _jwtService.GenerateToken(
     user.Id,
     user.Email);

        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return Result.Success((object)new LoginResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken
        });
    }
    public async Task<Result> RefreshTokenAsync(
    RefreshTokenRequestDto request)
    {
        var refreshToken = await _refreshTokenRepository
            .GetByTokenAsync(request.RefreshToken);

        if (refreshToken == null)
        {
            return Result.Failure("Invalid refresh token.");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            return Result.Failure("Refresh token has expired.");
        }

        if (refreshToken.IsRevoked)
        {
            return Result.Failure("Refresh token has been revoked.");
        }

        var newAccessToken = _jwtService.GenerateToken(
            refreshToken.UserId,
            refreshToken.User.Email);
        return Result.Success((object)newAccessToken);
    }
}