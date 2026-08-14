namespace CleanAuth.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string email);
}