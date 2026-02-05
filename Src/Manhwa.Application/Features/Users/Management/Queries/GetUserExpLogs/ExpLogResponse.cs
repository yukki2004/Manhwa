using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserExpLogs
{
    public class ExpLogResponse
    {
        public long ExpLogId { get; set; }
        public string Action { get; set; } = null!; 
        public int ExpAmount { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
    }
}
