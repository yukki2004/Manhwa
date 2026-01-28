using Manhwa.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace Manhwa.Infrastructure.Rankings
{

        public abstract class BaseInteractionStrategy
        {
            protected readonly ICacheService _cache;
            protected BaseInteractionStrategy(ICacheService cache) => _cache = cache;

            protected async Task UpdateZSetRanking(long storyId, double score, CancellationToken ct = default)
            {
                var now = DateTimeOffset.UtcNow;
                string storyIdStr = storyId.ToString();

                var rankings = new[]
                {
                new { Key = "ranking:all_time", TTL = (TimeSpan?)null },
                new { Key = $"ranking:daily:{now:yyyyMMdd}", TTL = (TimeSpan?)TimeSpan.FromDays(2) },
                new { Key = $"ranking:weekly:{GetIso8601WeekOfYear(now)}", TTL = (TimeSpan?)TimeSpan.FromDays(14) },
                new { Key = $"ranking:monthly:{now:yyyyMM}", TTL = (TimeSpan?)TimeSpan.FromDays(60) }
            };

                var tasks = rankings.Select(async r =>
                {
                    await _cache.SortedSetIncrementAsync(r.Key, storyIdStr, score, ct);

                    if (r.TTL.HasValue)
                    {
                        await _cache.SetExpirationAsync(r.Key, r.TTL.Value, ct);
                    }
                });
                await Task.WhenAll(tasks);
            }

            private string GetIso8601WeekOfYear(DateTimeOffset time)
            {
                int week = ISOWeek.GetWeekOfYear(time.UtcDateTime);
                int year = ISOWeek.GetYear(time.UtcDateTime);

                return $"{year}-W{week:D2}";
            }
        }
    
}
