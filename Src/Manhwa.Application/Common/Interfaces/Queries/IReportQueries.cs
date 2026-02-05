using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Reports.Queries.GetReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IReportQueries
    {
        Task<PagedResult<ReportRawDto>> GetPagedReportsAsync(GetReportsQuery request, CancellationToken ct);
    }
}
