using Manhwa.Application.Common.Interfaces;
using Manhwa.Infrastructure.Realtime.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Realtime.Services
{
    public class RealtimeService : IRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealtimeService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(long userId, string method, object data)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync(method, data);
        }

        public async Task SendToGroupAsync(string groupName, string method, object data)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, data);
        }
    }
}
