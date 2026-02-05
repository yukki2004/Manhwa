using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Report;
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
    public class CommentReportStrategy : IReportTargetStrategy
    {
        private readonly AppDbContext _context;
        public CommentReportStrategy(AppDbContext context) => _context = context;
        public ReportTargetType TargetType => ReportTargetType.Comment;

        public async Task<(string TargetName, string RedirectUrl, string? AvatarUrl)> ResolveAsync(long targetId, CancellationToken ct)
        {
            var comment = await _context.Comments.AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Story)
                .Include(c => c.Chapter).ThenInclude(ch => ch.Story)
                .Where(c => c.CommentId == targetId)
                .FirstOrDefaultAsync(ct);

            string url = comment?.ChapterId != null
                ? $"/truyen/{comment.Chapter.Story.Slug}/{comment.Chapter.Slug}?cmt={targetId}"
                : $"/truyen/{comment?.Story?.Slug}?cmt={targetId}";

            string contentSnippet = comment?.Content?.Length > 30 ? comment.Content[..30] + "..." : comment?.Content ?? "N/A";

            return (contentSnippet, url, comment?.User?.Avatar.ToFullUrl());
        }
    }
}
