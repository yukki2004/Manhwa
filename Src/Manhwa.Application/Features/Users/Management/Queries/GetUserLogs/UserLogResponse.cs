using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserLogs
{
    public class UserLogResponse
    {
        public long UserLogId { get; set; }
        public string Action { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string UserAgent { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
