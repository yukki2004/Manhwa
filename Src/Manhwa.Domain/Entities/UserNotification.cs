using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class UserNotification
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset? ReadAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Notification Notification { get; set; } = null!;
    }
}
