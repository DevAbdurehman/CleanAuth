using CleanAuth.Application.Interfaces.Repositories;
using CleanAuth.Domain.Entities;
using CleanAuth.Infrastructure.Data;

namespace CleanAuth.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDBContext _context;

    public RefreshTokenRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }
}