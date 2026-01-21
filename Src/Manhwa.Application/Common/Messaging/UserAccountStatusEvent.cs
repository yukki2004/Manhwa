using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class UserAccountStatusEvent
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public UserLogAction Action { get; set; }
        public DateTimeOffset CreateAt { get; set; }
    }
}
