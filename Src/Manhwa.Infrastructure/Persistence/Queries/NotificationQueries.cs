using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Common.Mappings;
using Manhwa.Application.Features.Notifications.Queries.GetNotifications;
using Manhwa.Domain.Entities;
using Manhwa.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class NotificationQueries : INotificationQueries
    {
        private readonly AppDbContext _context;
        public NotificationQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetUnreadCountAsync(long userId, CancellationToken ct)
        {
            return await _context.Set<UserNotification>()
                .CountAsync(un => un.UserId == userId && !un.IsRead, ct); 
        }

        public async Task<PagedResult<NotificationResult>> GetPagedNotificationsAsync(GetNotificationsQuery request, CancellationToken ct)
        {
            var query = _context.Set<UserNotification>()
                .AsNoTracking()
                .Include(un => un.Notification)
                    .ThenInclude(n => n.Sender)
                .Where(un => un.UserId == request.UserId)
                .OrderByDescending(un => un.CreatedAt); 

            var pagedData = await query.ToPagedListAsync(request.PageIndex, request.PageSize, ct);

            var items = pagedData.Items.Select(un =>
                NotificationMapping.MapToResult(un.Notification, un.IsRead)).ToList();

            return new PagedResult<NotificationResult>(items, pagedData.TotalCount, request.PageIndex, request.PageSize);
        }
    }
}
