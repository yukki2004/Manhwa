using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Command.MarkAsRead
{
    public class MarkAsReadCommand : IRequest<bool>
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
    }
}
