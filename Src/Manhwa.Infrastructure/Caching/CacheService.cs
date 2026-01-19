using Manhwa.Application.Common.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return typeof(T) == typeof(string)
                ? (T?)(object)value.ToString()
                : JsonSerializer.Deserialize<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
        {
            var stringValue = typeof(T) == typeof(string)
                ? value?.ToString()
                : JsonSerializer.Serialize(value);

            await _database.StringSetAsync(key, stringValue, expiration);
        }
        public async Task RemoveAsync(string key, CancellationToken ct = default)
        {
            await _database.KeyDeleteAsync(key);
        }
        public async Task<bool> ExistsAsync(string key, CancellationToken ct = default)
        {
            return await _database.KeyExistsAsync(key);
        }
    }
}
