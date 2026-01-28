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
    public class ReadingHistoryRepository : IReadingHistoryRepository
    {
        private readonly AppDbContext _context;

        public ReadingHistoryRepository(AppDbContext context) => _context = context;

        public async Task UpsertHistoryAsync(long userId, long storyId, long chapterId)
        {
            var existing = await _context.ReadingHistories
                .FirstOrDefaultAsync(h => h.UserId == userId && h.StoryId == storyId && h.ChapterId == chapterId);

            if (existing != null)
            {
                existing.LastReadAt = DateTimeOffset.UtcNow;
            }
            else
            {
                await _context.ReadingHistories.AddAsync(new ReadingHistory
                {
                    UserId = userId,
                    StoryId = storyId,
                    ChapterId = chapterId,
                    LastReadAt = DateTimeOffset.UtcNow
                });
            }

        }
    }
}
