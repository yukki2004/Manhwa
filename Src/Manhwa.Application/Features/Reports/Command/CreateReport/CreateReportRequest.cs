using Manhwa.Domain.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Command.CreateReport
{
    public class CreateReportRequest
    {
        public long TargetId { get; set; }
        public ReportTargetType TargetType { get; set; }
        public string Reason { get; set; } = null!;
    }
}
