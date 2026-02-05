using Manhwa.Domain.Enums.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Command.CreateReport
{
    public class CreateReportCommand : IRequest<bool>
    {
        public long UserId { get; set; }
        public long TargetId { get; set; }
        public ReportTargetType TargetType { get; set; }
        public string Reason { get; set; } = null!;
    }
}
