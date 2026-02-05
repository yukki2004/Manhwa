using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Command.ProcessReport
{
    public class ProcessReportCommand : IRequest<bool>
    {
        public long ReportId { get; set; }

    }
}
