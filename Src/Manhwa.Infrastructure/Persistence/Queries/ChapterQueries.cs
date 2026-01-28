using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Chapters.Queries.ViewChapter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class ChapterQueries : IChapterQueries
    {
        private readonly AppDbContext _context;

        public ChapterQueries(AppDbContext context) => _context = context;

        public async Task<ChapterViewDto?> GetChapterDetailAsync(string storySlug, string chapterSlug, CancellationToken ct)
        {
            return await _context.Chapters
                .AsNoTracking()
                .Where(c => c.Slug == chapterSlug && c.Story.Slug == storySlug)
                .Include(c => c.Story)
                .Include(c => c.ChapterImages)
                .Select(c => new ChapterViewDto
                {
                    ChapterId = c.ChapterId,
                    Title = c.Title ?? "",
                    ChapterSlug = c.Slug,
                    StoryId = c.StoryId,
                    StoryTitle = c.Story.Title,
                    StorySlug = c.Story.Slug,
                    ImageUrls = c.ChapterImages.Select(im => new ImageUrlDto
                    {
                        Url = $"{im.ImageUrl.ToFullUrl()}?v={(c.UpdatedAt).Ticks}",
                        OrderIndex = im.OrderIndex
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }


    }
}
