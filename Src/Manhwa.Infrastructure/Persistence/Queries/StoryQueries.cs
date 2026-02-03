using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Stories.Queries.GetHomeStories;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Story;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class StoryQueries : IStoryQueries
    {
        private readonly AppDbContext _context;
        public StoryQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<HomeStoryDto>> GetPagedHomeStoriesAsync(int pageIndex, int pageSize, CancellationToken ct)
        {
            return await _context.Set<Story>()
                .AsNoTracking()
                .Where(s => s.IsPublish == StoryPublishStatus.Published)
                .OrderByDescending(s => s.UpdatedAt)
                .Select(s => new HomeStoryDto
                {
                    StoryId = s.StoryId,
                    Title = s.Title,
                    Slug = s.Slug,
                    Thumbnail = s.ThumbnailUrl.ToFullUrl(),
                    RateAvg = s.RateAvg,
                    RateCount = s.RateCount,
                    UpdatedAt = s.UpdatedAt,
                    Author = s.Author ?? "Ẩn danh",
                    ShortDescription = s.Description != null && s.Description.Length > 160
                        ? s.Description.Substring(0, 160) + "..."
                        : s.Description,

                    Genres = s.StoryCategories.Select(c => c.Category.Name).ToList(),

                    RecentChapters = s.Chapters
                        .OrderByDescending(c => c.CreatedAt)
                        .Take(3)
                        .Select(c => new HomeRecentChapterDto
                        {
                            ChapterId = c.ChapterId,
                            Slug = c.Slug,
                            Title = c.Title ?? c.ChapterNumber.ToString(),
                            CreateAt = c.CreatedAt,
                        })
                        .ToList()
                })
                .ToPagedListAsync(pageIndex, pageSize, ct);
        }
    }
}
