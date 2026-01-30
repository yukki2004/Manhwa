using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<(List<string> IDs, int TotalCount)> GetSortedSetPagedAsync(string key, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            long start = (pageIndex - 1) * pageSize;
            long stop = start + pageSize - 1;
            var members = await _database.SortedSetRangeByRankAsync(key, start, stop, Order.Descending);
            var total = await _database.SortedSetLengthAsync(key);

            return (members.Select(m => m.ToString()).ToList(), (int)total);
        }

        public async Task<string> ResolveRankingKeyAsync(RankingType type, CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow;
            string prefix = type switch
            {
                RankingType.Daily => "ranking:daily:",
                RankingType.Weekly => "ranking:weekly:",
                RankingType.Monthly => "ranking:monthly:",
                RankingType.AllTime => "ranking:all_time",
                _ => "ranking:all_time"
            };

            if (type == RankingType.AllTime) return prefix;

            string targetKey = type switch
            {
                RankingType.Daily => $"{prefix}{now:yyyyMMdd}",
                RankingType.Weekly => $"{prefix}{ISOWeek.GetYear(now.UtcDateTime)}-W{ISOWeek.GetWeekOfYear(now.UtcDateTime):D2}",
                RankingType.Monthly => $"{prefix}{now:yyyyMM}",
                _ => prefix
            };

            if (await _database.KeyExistsAsync(targetKey) && await _database.SortedSetLengthAsync(targetKey) > 0)
            {
                return targetKey;
            }

            return await GetLatestKeyByPatternAsync(prefix + "*", ct) ?? targetKey;
        }

        public async Task<string?> GetLatestKeyByPatternAsync(string pattern, CancellationToken ct = default)
        {
            var allKeys = await GetKeysAsync(pattern);
            var sortedKeys = allKeys.OrderByDescending(k => k).ToList();

            foreach (var key in sortedKeys)
            {
                if (await _database.SortedSetLengthAsync(key) > 0)
                {
                    return key;
                }
            }
            return null;
        }
    }

}

