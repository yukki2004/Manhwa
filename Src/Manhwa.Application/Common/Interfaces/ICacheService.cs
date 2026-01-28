using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);
        Task RemoveAsync(string key, CancellationToken ct = default);
        Task<bool> ExistsAsync(string key, CancellationToken ct = default);
        Task<long> IncrementAsync(string key, long value = 1, CancellationToken ct = default);
        Task<double> SortedSetIncrementAsync(string key, string member, double value, CancellationToken ct = default);
        Task<bool> SetExpirationAsync(string key, TimeSpan expiration, CancellationToken ct = default);
        Task<long> DecrementAsync(string key, long value = 1, CancellationToken ct = default);
        Task<IEnumerable<string>> GetKeysAsync(string pattern);
        Task<long> GetAndDeleteViewAsync(string key);

    }
}
