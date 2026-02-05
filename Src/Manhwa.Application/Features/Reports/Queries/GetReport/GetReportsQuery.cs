using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using Manhwa.Domain.Enums.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Queries.GetReport
{
    public class GetReportsQuery : PagingParamUsers, IRequest<PagedResult<ReportResult>>
    {
        public ReportStatus? Status { get; init; }
        public ReportTargetType? TargetType { get; init; }
    }
}
