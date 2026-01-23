using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces
{
    public interface IRealtimeService
    {
        Task SendToUserAsync(long userId, string method, object data);
        Task SendToGroupAsync(string groupName, string method, object data);
    }
}
