using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Stories.Queries.GetFilteredStories;
using Manhwa.Application.Features.Stories.Queries.GetHomeStories;
using Manhwa.Application.Features.Stories.Queries.GetStoryDetail;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Story;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

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
        public async Task<PagedResult<FilteredStoryDto>> GetFilteredStoriesAsync(FilterStoriesQuery request, CancellationToken ct)
        {
            var query = _context.Set<Story>()
                .AsNoTracking()
                .Where(s => s.IsPublish == StoryPublishStatus.Published);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(s => s.Title.ToLower().Contains(term) || s.Slug.Contains(term));
            }

            if (request.CategorySlugs != null && request.CategorySlugs.Any())
            {
                foreach (var slug in request.CategorySlugs)
                {
                    var targetSlug = slug.ToLower().Trim();
                    query = query.Where(s => s.StoryCategories
                        .Any(sc => sc.Category.Slug.ToLower() == targetSlug));
                }
            }

            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(s => s.ReleaseYear == request.ReleaseYear.Value);
            }

            if (request.MinChapters.HasValue)
            {
                query = query.Where(s => s.Chapters.Count >= request.MinChapters.Value);
            }

            query = request.SortBy?.ToLower() switch
            {
                "views" => query.OrderByDescending(s => s.TotalView),
                "rating" => query.OrderByDescending(s => s.RateAvg),
                _ => query.OrderByDescending(s => s.UpdatedAt)
            };

            return await query.Select(s => new FilteredStoryDto
            {
                StoryId = s.StoryId,
                Title = s.Title,
                Slug = s.Slug,
                Thumbnail = s.ThumbnailUrl.ToFullUrl(),
                RateAvg = s.RateAvg,
                Author = s.Author,
                ShortDescription = s.Description != null && s.Description.Length > 150
                    ? s.Description.Substring(0, 150) + "..."
                    : s.Description,
                Genres = s.StoryCategories.Select(sc => sc.Category.Name).ToList(),
                RecentChapters = s.Chapters
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .Select(c => new FilteredRecentChapterDto
                    {
                        ChapterId = c.ChapterId,
                        Slug = c.Slug,
                        Title = c.Title ?? c.ChapterNumber.ToString(),
                        CreateAt = c.CreatedAt,
                    })
                    .ToList()
            }).ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }

        public async Task<StoryDetailResponse?> GetStoryDetailWithChaptersAsync(GetStoryDetailQuery request, CancellationToken ct)
        {
            var storyInfo = await _context.Set<Story>()
                .AsNoTracking()
                .Where(s => s.Slug == request.Slug && s.IsPublish == StoryPublishStatus.Published)
                .Select(s => new {
                    s.StoryId,
                    s.Title,
                    s.Slug,
                    s.Description,
                    s.Author,
                    Thumbnail = s.ThumbnailUrl,
                    s.RateAvg,
                    s.ReleaseYear,
                    s.CreatedAt,
                    s.TotalView,
                    s.RateCount,
                    TotalChapters = s.Chapters.Count(), 
                    Genres = s.StoryCategories.Select(sc => new CategoryStoryDetailDto
                    {
                        Name = sc.Category.Name,
                        Slug = sc.Category.Slug,
                    }).ToList() 
                })
                .FirstOrDefaultAsync(ct);

            if (storyInfo == null) return null;

            var chaptersPaged = await _context.Set<Chapter>()
                .AsNoTracking()
                .Where(c => c.StoryId == storyInfo.StoryId)
                .OrderByDescending(c => c.ChapterNumber)
                .Select(c => new ChapterItemDto
                {
                    ChapterId = c.ChapterId,
                    ChapterNumber = c.ChapterNumber,
                    Title = c.Title ?? $"Chương {c.ChapterNumber}",
                    Slug = c.Slug,
                    CreateAt = c.CreatedAt 
                })
                .ToPagedListAsync(request.PageIndex, request.PageSize, ct); 

            return new StoryDetailResponse
            {
                StoryId = storyInfo.StoryId,
                Title = storyInfo.Title,
                Slug = storyInfo.Slug,
                Description = storyInfo.Description,
                Author = storyInfo.Author,
                CreateAt = storyInfo.CreatedAt,
                Realease_year = storyInfo.ReleaseYear,
                TotalView = storyInfo.TotalView,
                Thumbnail = storyInfo.Thumbnail.ToFullUrl(),
                RateAvg = storyInfo.RateAvg,
                RateCount = storyInfo.RateCount,
                TotalChapters = storyInfo.TotalChapters,
                Genres = storyInfo.Genres,
                Chapters = chaptersPaged
            };
        }
    }
}
