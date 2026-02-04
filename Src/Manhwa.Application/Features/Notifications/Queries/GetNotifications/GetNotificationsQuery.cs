using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery : PagingParamUsers, IRequest<PagedResult<NotificationResult>>
    {
        public long UserId { get; set; }
    }
}
