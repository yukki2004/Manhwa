using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Notifications.Queries.GetNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface INotificationQueries
    {
        Task<PagedResult<NotificationResult>> GetPagedNotificationsAsync(GetNotificationsQuery request, CancellationToken ct);

        Task<int> GetUnreadCountAsync(long userId, CancellationToken ct);
    }
}
