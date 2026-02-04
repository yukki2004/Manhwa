using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Command.MarkAllAsRead
{
    public class MarkAllAsReadCommand : IRequest<bool>
    {
        public long UserId { get; set; }
    }
}
