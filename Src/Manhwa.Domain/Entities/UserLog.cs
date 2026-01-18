using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class UserLog
    {
        public long UserLogId { get; set; }
        public string? IpAddress { get; set; }
        public UserLogAction Action { get; set; }
        public string UserAgent { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public long UserId { get; set; }
        // Navigation property
        public User User { get; set; } = null!;

    }
}
