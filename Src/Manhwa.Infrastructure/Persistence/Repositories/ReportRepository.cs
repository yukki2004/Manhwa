using Manhwa.Application.Common.Extensions;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Report;
using Manhwa.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Report report, CancellationToken ct = default)
        {
            await _context.Set<Report>().AddAsync(report, ct);
        }

        public async Task<Report?> GetByIdAsync(long reportId, CancellationToken ct = default)
        {
            return await _context.Set<Report>()
                .Include(r => r.User) 
                .FirstOrDefaultAsync(r => r.ReportId == reportId, ct);
        }

        public async Task UpdateStatusAsync(long reportId, ReportStatus status, CancellationToken ct = default)
        {
            await _context.Set<Report>()
                .Where(r => r.ReportId == reportId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.Status, status), ct);
        }

        public async Task<bool> IsAlreadyReportedAsync(long userId, long targetId, ReportTargetType targetType, CancellationToken ct = default)
        {
            return await _context.Set<Report>()
                .AnyAsync(r => r.UserId == userId &&
                               r.TargetId == targetId &&
                               r.TargetType == targetType &&
                               r.Status == ReportStatus.Pending, ct);
        }
        public async Task<string?> GetSnapshotAsync(ReportTargetType type, long id, CancellationToken ct)
        {
            object? snapshotData = null;

            switch (type)
            {
                case ReportTargetType.Story:
                    snapshotData = await _context.Stories
                        .AsNoTracking()
                        .Where(s => s.StoryId == id)
                        .Select(s => new {
                            s.Title,
                            s.Author,
                            s.Slug,
                            s.ThumbnailUrl,
                            SnapshotAt = DateTimeOffset.UtcNow,
                        })
                        .FirstOrDefaultAsync(ct);
                    break;

                case ReportTargetType.Chapter:
                    snapshotData = await _context.Chapters
                        .AsNoTracking()
                        .Include(c => c.Story) 
                        .Where(c => c.ChapterId == id)
                        .Select(c => new {
                            StoryTitle = c.Story.Title,
                            ChapterTitle = c.Title,
                            ChapterNumber = c.ChapterNumber,
                            SnapshotAt = DateTimeOffset.UtcNow
                        })
                        .FirstOrDefaultAsync(ct);
                    break;

                case ReportTargetType.Comment:
                    snapshotData = await _context.Comments
                        .AsNoTracking()
                        .Include(c => c.User)
                        .Where(c => c.CommentId == id)
                        .Select(c => new {
                            Author = c.User.Username,
                            Content = c.Content,
                            CommentCreatedAt = c.CreatedAt,
                            SnapshotAt = DateTimeOffset.UtcNow
                        })
                        .FirstOrDefaultAsync(ct);
                    break;

                case ReportTargetType.User:
                    snapshotData = await _context.Users
                        .AsNoTracking()
                        .Where(u => u.UserId == id)
                        .Select(u => new {
                            u.Username,
                            u.Email,
                            u.Role,
                            u.Avatar,
                            SnapshotAt = DateTimeOffset.UtcNow,
                        })
                        .FirstOrDefaultAsync(ct);
                    break;
            }

            return snapshotData != null
                ? JsonSerializer.Serialize(snapshotData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                : null;
        }
    }
}
