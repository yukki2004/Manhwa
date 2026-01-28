using Manhwa.Application.Common.Interfaces;
using Manhwa.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.BackgroundTasks
{
    public class ViewSyncWorker : BackgroundService
    {
        private readonly ICacheService _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ViewSyncWorker> _logger;

        public ViewSyncWorker(
            ICacheService cache,
            IServiceScopeFactory scopeFactory,
            ILogger<ViewSyncWorker> logger)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker đồng bộ View đã bắt đầu.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("--- Chu kỳ đồng bộ bắt đầu ---");
                    await SyncAllViewsToDb();
                    _logger.LogInformation("--- Chu kỳ đồng bộ kết thúc thành công ---");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi đồng bộ View từ Redis về Database.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task SyncAllViewsToDb()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await ProcessSync(context, "count:story:views:*", "stories", "story_id");

            await ProcessSync(context, "count:chapter:views:*", "chapters", "chapter_id");
        }

        private async Task ProcessSync(AppDbContext context, string pattern, string tableName, string primaryKeyName)
        {
            var keys = await _cache.GetKeysAsync(pattern);

            foreach (var key in keys)
            {
                if (!long.TryParse(key.Split(':').Last(), out long id)) continue;

                long viewsToAdd = await _cache.GetAndDeleteViewAsync(key);

                if (viewsToAdd > 0)
                {
                    var sql = $"UPDATE {tableName} SET total_view = total_view + @p0 WHERE {primaryKeyName} = @p1";
                    await context.Database.ExecuteSqlRawAsync(sql, viewsToAdd, id);
                    _logger.LogDebug("Đã cộng {views} view vào {table} ID {id}", viewsToAdd, tableName, id);
                }
            }
        }
    }
}
