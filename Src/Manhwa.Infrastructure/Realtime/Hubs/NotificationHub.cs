using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Realtime.Hubs
{
    [Authorize] 
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            await base.OnConnectedAsync();
        }

        public async Task JoinStoryGroup(long storyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Story_{storyId}");
        }

        public async Task LeaveStoryGroup(long storyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Story_{storyId}");
        }
    }
}
