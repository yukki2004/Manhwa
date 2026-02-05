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
    public class StoryReportStrategy : IReportTargetStrategy
    {
        private readonly AppDbContext _context;
        public StoryReportStrategy(AppDbContext context) => _context = context;

        public ReportTargetType TargetType => ReportTargetType.Story;

        public async Task<(string TargetName, string RedirectUrl, string? AvatarUrl)> ResolveAsync(long targetId, CancellationToken ct)
        {
            var story = await _context.Stories.AsNoTracking()
                .Where(s => s.StoryId == targetId)
                .Select(s => new { s.Title, s.Slug, s.ThumbnailUrl })
                .FirstOrDefaultAsync(ct);

            return (story?.Title ?? "N/A", $"/truyen/{story?.Slug}", story?.ThumbnailUrl);
        }
    }
}
