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
        private readonly IConnectionMultiplexer _redis; 
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _redis = redis; 
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

        public async Task<long> IncrementAsync(string key, long value = 1, CancellationToken ct = default)
        {
            return await _database.StringIncrementAsync(key, value);
        }
        public async Task<double> SortedSetIncrementAsync(string key, string member, double value, CancellationToken ct = default)
        {
            return await _database.SortedSetIncrementAsync(key, member, value);
        }
        public async Task<bool> SetExpirationAsync(string key, TimeSpan expiration, CancellationToken ct = default)
        {
            return await _database.KeyExpireAsync(key, expiration, ExpireWhen.HasNoExpiry);
        }
        public async Task<long> DecrementAsync(string key, long value = 1, CancellationToken ct = default)
        {
            return await _database.StringDecrementAsync(key, value);
        }
        public async Task<long> GetAndDeleteViewAsync(string key)
        {
            var luaScript = @"
            local val = redis.call('GET', KEYS[1])
            if val then
                redis.call('DEL', KEYS[1])
                return tonumber(val)
            else
                return 0
            end";

            var result = await _database.ScriptEvaluateAsync(luaScript, new RedisKey[] { key });
            return (long)result;
        }
        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            var keys = new List<string>();
            foreach (var endpoint in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    keys.Add(key.ToString());
                }
            }
            return keys.Distinct();
        }

    }
}
