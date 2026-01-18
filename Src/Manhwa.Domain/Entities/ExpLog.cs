using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhwa.Domain.Enums;

namespace Manhwa.Domain.Entities
{
    public class ExpLog
    {
        public long ExpLogId { get; set; }
        public ExpActionType Action { get; set; }
        public int ExpAmount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long UserId { get; set; }
        // Navigation property
        public User User { get; set; } = null!;

    }
}
