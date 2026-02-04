using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification, CancellationToken ct = default);
        Task AddUserNotificationsAsync(IEnumerable<UserNotification> userNotifications, CancellationToken ct = default);
        Task<User?> GetSenderAsync(long senderId, CancellationToken ct = default);
        Task MarkAsReadAsync(long userId, long notificationId, CancellationToken ct);
        Task MarkAllAsReadAsync(long userId, CancellationToken ct);
    }
}
