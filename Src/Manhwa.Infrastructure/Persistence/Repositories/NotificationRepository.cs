using Manhwa.Application.Common.Extensions;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Notification userNotification, CancellationToken ct = default)
        {
            await _context.Notifications.AddAsync(userNotification, ct);
        }

        public async Task AddUserNotificationsAsync(IEnumerable<UserNotification> userNotifications, CancellationToken ct = default)
        {
            await _context.Set<UserNotification>().AddRangeAsync(userNotifications, ct);
        }
        public async Task<User?> GetSenderAsync(long senderId, CancellationToken ct = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == senderId)
                .Select(u => new User
                {
                    Username = u.Username,
                    Avatar = u.Avatar.ToFullUrl(),
                })
                .FirstOrDefaultAsync(ct);
        }
        public async Task MarkAsReadAsync(long userId, long notificationId, CancellationToken ct)
        {
            await _context.Set<UserNotification>()
                .Where(un => un.UserId == userId && un.NotificationId == notificationId && !un.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(un => un.IsRead, true), ct);
        }

        public async Task MarkAllAsReadAsync(long userId, CancellationToken ct)
        {
            await _context.Set<UserNotification>()
                .Where(un => un.UserId == userId && !un.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(un => un.IsRead, true), ct);
        }
    }
}
