using Manhwa.Domain.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Queries.GetReport
{
    public class ReportRawDto
    {
        public long ReportId { get; set; }
        public string Reason { get; set; } = null!;
        public ReportStatus Status { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public long TargetId { get; set; }
        public ReportTargetType TargetType { get; set; }
        public string ReporterName { get; set; } = null!;
    }
}
