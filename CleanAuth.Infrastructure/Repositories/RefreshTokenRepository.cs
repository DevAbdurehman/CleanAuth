using CleanAuth.Application.Interfaces.Repositories;
using CleanAuth.Domain.Entities;
using CleanAuth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }
}