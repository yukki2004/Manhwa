using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountQuery : IRequest<int>
    {
        public long UserId { get; set; }
    }
}
