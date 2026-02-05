using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Report;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Report;
using Manhwa.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Reports
{
    public class ChapterReportStrategy : IReportTargetStrategy
    {
        private readonly AppDbContext _context;
        public ChapterReportStrategy(AppDbContext context) => _context = context;

        public ReportTargetType TargetType => ReportTargetType.Chapter;

        public async Task<(string TargetName, string RedirectUrl, string? AvatarUrl)> ResolveAsync(long targetId, CancellationToken ct)
        {
            var chapter = await _context.Chapters.AsNoTracking()
                .Include(c => c.Story)
                .Where(c => c.ChapterId == targetId)
                .Select(c => new { c.Title, c.Slug, StoryTitle = c.Story.Title, StorySlug = c.Story.Slug, c.Story.ThumbnailUrl })
                .FirstOrDefaultAsync(ct);

            return ($"{chapter?.StoryTitle} - {chapter?.Title}",
                    $"/truyen/{chapter?.StorySlug}/{chapter?.Slug}",
                    chapter?.ThumbnailUrl.ToFullUrl());
        }
    }
}
