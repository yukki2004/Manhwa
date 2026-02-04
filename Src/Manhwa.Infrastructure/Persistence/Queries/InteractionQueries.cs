using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Interactions.Queries.GetComments;
using Manhwa.Application.Features.Interactions.Queries.GetReplyComments;
using Manhwa.Domain.Entities;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class InteractionQueries : IInteractionQueries
    {
        private readonly AppDbContext _context;
        public InteractionQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<CommentDto>> GetPagedRootCommentsAsync(GetRootCommentsQuery request, CancellationToken ct)
        {
            var query = _context.Set<Comment>()
                .AsNoTracking()
                .Where(c => c.ParentId == null && c.Status == Domain.Enums.Comment.CommentStatus.Published);

            if (!string.IsNullOrEmpty(request.ChapterSlug))
            {
                query = query.Where(c => c.Chapter != null
                                      && c.Chapter.Slug == request.ChapterSlug
                                      && c.Chapter.Story.Slug == request.StorySlug);
            }
            else
            {
                query = query.Where(c => c.Story.Slug == request.StorySlug);
            }

            return await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    CreateAt = c.CreatedAt,
                    UserName = c.User.Username ?? "ẩn danh",
                    UserAvatar = c.User.Avatar.ToFullUrl(),
                    Level = c.User.Level,
                    ChapterNumber = c.Chapter != null ? c.Chapter.ChapterNumber : null,
                    ChapterSlug = c.Chapter != null ? c.Chapter.Slug : null,
                    ReplyCount = _context.Set<Comment>().Count(r => r.ParentId == c.CommentId)
                })
                .ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }
        public async Task<PagedResult<CommentReplyDto>> GetPagedRepliesAsync(GetRepliesQuery request, CancellationToken ct)
        {
            var query = _context.Set<Comment>()
                .AsNoTracking()
                .Where(c => c.ParentId == request.ParentId && c.Status == Domain.Enums.Comment.CommentStatus.Published);

            return await query
                .OrderBy(c => c.CreatedAt) 
                .Select(c => new CommentReplyDto
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    CreateAt = c.CreatedAt,
                    ParentId = c.ParentId,
                    ReplyCount = _context.Set<Comment>().Count(sub => sub.ParentId == c.CommentId),
                    UserName = c.User.Username ?? "ẩn danh",
                    UserAvatar = c.User.Avatar.ToFullUrl(),
                    Level = c.User.Level,

                    RepliedToUserName = c.Parent != null ? (c.Parent.User.Username ?? "ẩn danh") : null
                })
                .ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }
    }
}
