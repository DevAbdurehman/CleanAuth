using CleanAuth.Application.Common;
using CleanAuth.Application.DTOs.Auth;

namespace CleanAuth.Application.Services.Interfaces;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequestDto request);
}