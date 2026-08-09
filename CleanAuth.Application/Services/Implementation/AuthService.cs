
using CleanAuth.Application.Common;
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

    public AuthService(
      IUserRepository userRepository,
      IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
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
}