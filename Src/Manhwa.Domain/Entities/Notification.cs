using Manhwa.Domain.Enums.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public NotificationType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        // Foreign Key & Navigation Property (Người gửi - thường là Admin hoặc Hệ thống)
        public long? SenderId { get; set; }
        public User? Sender { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
