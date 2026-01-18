using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rf => rf.Token == token , cancellationToken);
        }
        public async Task<RefreshToken?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens.FindAsync( id , cancellationToken);
        }
        public async Task DeleteAllUserTokensAsync(long userId, CancellationToken ct)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteDeleteAsync(ct);
        }
        public async Task MarkTokenAsUsedAsync(string token, CancellationToken ct)
        {
            await _context.RefreshTokens
                .Where(rt => rt.Token == token)
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsUsed, true), ct);
        }
    }
}
