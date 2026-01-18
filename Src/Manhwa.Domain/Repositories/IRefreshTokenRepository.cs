using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken , CancellationToken ct = default);
        Task<RefreshToken?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task DeleteAllUserTokensAsync(long userId, CancellationToken ct);
        Task MarkTokenAsUsedAsync(string token, CancellationToken ct);

    }
}
