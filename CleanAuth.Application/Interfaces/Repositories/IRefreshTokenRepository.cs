using CleanAuth.Domain.Entities;

namespace CleanAuth.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenAsync(string token);
}