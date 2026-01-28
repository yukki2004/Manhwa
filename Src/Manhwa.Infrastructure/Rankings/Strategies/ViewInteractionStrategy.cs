using Manhwa.Application.Common.Interfaces.Ranking;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhwa.Domain.Repositories;

namespace Manhwa.Infrastructure.Rankings.Strategies
{
    public class ViewInteractionStrategy : BaseInteractionStrategy, IInteractionStrategy
    {
        private readonly IReadingHistoryRepository _readingHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InteractionType Type => InteractionType.View;

        public ViewInteractionStrategy(ICacheService cache, IReadingHistoryRepository readingHistoryRepository, IUnitOfWork unitOfWork) : base(cache)
        {
            _readingHistoryRepository = readingHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(long storyId, long? chapterId, string identity)
        {
            if (chapterId == null)
            {
                throw new ArgumentNullException(nameof(chapterId), "ChapterId cannot be null for view interaction.");
            }
            var lockViewKey = $"lock:view:{chapterId}:{identity}";
            var isLocked = await _cache.ExistsAsync(lockViewKey);
            if (isLocked) return; 

            await _cache.SetAsync(lockViewKey, "1", TimeSpan.FromMinutes(10));

            await UpdateZSetRanking(storyId, 1);

            await _cache.IncrementAsync($"count:story:views:{storyId}");
            await _cache.IncrementAsync($"count:chapter:views:{chapterId}");

            if (identity.StartsWith("u_"))
            {
                var userId = long.Parse(identity.Replace("u_", ""));
                await _readingHistoryRepository.UpsertHistoryAsync(userId, storyId, (long)chapterId);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
