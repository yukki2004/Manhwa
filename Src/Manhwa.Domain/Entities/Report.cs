using Manhwa.Domain.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Report
    {
        public long ReportId { get; set; }
        public string Reason { get; set; } = null!;
        public ReportTargetType TargetType { get; set; } 
        public long TargetId { get; set; }
        public ReportStatus Status { get; set; } 
        public string? Metadata { get; set; } 

        public DateTimeOffset CreateAt { get; set; } 
        public long UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
