using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Abstractions
{
    public class ReportResult
    {
        public long ReportId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTimeOffset CreateAt { get; set; }
        public ReportedTarget Target { get; set; } = null!;
    }

    public class ReportedTarget
    {
        public long Id { get; set; }
        public string Type { get; set; } = null!;
        public string DisplayName { get; set; } = null!; 
        public string RedirectUrl { get; set; } = null!; 
        public string? AvatarUrl { get; set; } 
    }
}
