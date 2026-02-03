using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Users.Management.Queries.GetAllUsers;
using Manhwa.Application.Features.Users.Profile.Queries.GetFavorites;
using Manhwa.Application.Features.Users.Profile.Queries.GetReadingHistory;
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
    public class UserQueries : IUserQueries
    {
        private readonly AppDbContext _context;
        public UserQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<GetAllUsersDto>> GetAllUsersAsync(GetAllUsersQuery request, CancellationToken ct)
        {
            var query = _context.Users.AsNoTracking();
            if (request.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(search) ||
                                         u.Email.ToLower().Contains(search));
            }

            var projection = query.Select(u => new GetAllUsersDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Avatar = u.Avatar.ToFullUrl(),
                Description = u.Description,
                IsActive = u.IsActive,
                loginType = u.LoginType,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });

            return await projection
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }
        public async Task<PagedResult<UserFavoriteDto>> GetPagedFavoritesWithChaptersAsync(
    long userId, int pageIndex, int pageSize, CancellationToken ct)
        {
            return await _context.Set<UserFavorite>()
                .AsNoTracking()
                .Where(f => f.UserId == userId && f.Story.IsPublish == StoryPublishStatus.Published)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new UserFavoriteDto
                {
                    StoryId = f.StoryId,
                    Title = f.Story.Title,
                    Slug = f.Story.Slug,
                    Thumbnail = f.Story.ThumbnailUrl.ToFullUrl(),
                    RecentChapters = f.Story.Chapters
                        .OrderByDescending(c => c.CreatedAt)
                        .Take(3)
                        .Select(c => new RecentChapterDto
                        {
                            ChapterId = c.ChapterId,
                            Slug = c.Slug,
                            Title = c.Title ?? "chương mới",
                            CreateAt = c.CreatedAt,
                        }).ToList()
                })
                .ToPagedListAsync(pageIndex, pageSize, ct);
        }
        public async Task<PagedResult<ReadingHistoryDto>> GetPagedReadingHistoryAsync(
            long userId, int pageIndex, int pageSize, CancellationToken ct)
        {
            var baseQuery = _context.Set<ReadingHistory>()
                .AsNoTracking()
                .Where(rh => rh.UserId == userId && rh.Story.IsPublish == StoryPublishStatus.Published);

            var latestHistoryQuery = baseQuery.Where(rh => rh.LastReadAt == _context.Set<ReadingHistory>()
                .Where(inner => inner.UserId == userId && inner.StoryId == rh.StoryId)
                .Max(inner => inner.LastReadAt));

            var finalQuery = latestHistoryQuery
                .OrderByDescending(rh => rh.LastReadAt)
                .Select(rh => new ReadingHistoryDto
                {
                    StoryId = rh.StoryId,
                    StoryTitle = rh.Story.Title,
                    StorySlug = rh.Story.Slug,
                    Thumbnail = rh.Story.ThumbnailUrl.ToFullUrl(),
                    ChapterId = rh.ChapterId,
                    ChapterTitle = rh.Chapter.Title ?? "Chương " + rh.Chapter.ChapterNumber,
                    ChapterSlug = rh.Chapter.Slug,
                    LastReadAt = rh.LastReadAt
                });

            return await finalQuery.ToPagedListAsync(pageIndex, pageSize, ct);
        }
    }
}