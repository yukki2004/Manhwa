using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class SendNotificationEvent
    {
        public List<long> ReceiverIds { get; set; } = new();
        public long? SenderId { get; set; }
        public NotificationType Type { get; set; } 
        public string? RawDataJson { get; set; }
    }
}
