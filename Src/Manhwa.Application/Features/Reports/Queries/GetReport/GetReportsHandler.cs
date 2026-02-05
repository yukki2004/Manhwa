using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Common.Interfaces.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Queries.GetReport
{
    public class GetReportsHandler : IRequestHandler<GetReportsQuery, PagedResult<ReportResult>>
    {
        private readonly IReportQueries _reportQueries;
        private readonly IEnumerable<IReportTargetStrategy> _strategies;

        public GetReportsHandler(IReportQueries reportQueries, IEnumerable<IReportTargetStrategy> strategies)
        {
            _reportQueries = reportQueries;
            _strategies = strategies;
        }

        public async Task<PagedResult<ReportResult>> Handle(GetReportsQuery request, CancellationToken ct)
        {
            var pagedRaw = await _reportQueries.GetPagedReportsAsync(request, ct);

            var finalItems = new List<ReportResult>();

            foreach (var raw in pagedRaw.Items)
            {
                var item = new ReportResult
                {
                    ReportId = raw.ReportId,
                    Reason = raw.Reason,
                    Status = raw.Status.ToString(),
                    CreateAt = raw.CreateAt,
                    Target = new ReportedTarget
                    {
                        Id = raw.TargetId,
                        Type = raw.TargetType.ToString()
                    }
                };

                var strategy = _strategies.FirstOrDefault(s => s.TargetType == raw.TargetType);
                if (strategy != null)
                {
                    var (name, url, avatar) = await strategy.ResolveAsync(raw.TargetId, ct);
                    item.Target.DisplayName = name;
                    item.Target.RedirectUrl = url;
                    item.Target.AvatarUrl = avatar;
                }

                finalItems.Add(item);
            }

            return new PagedResult<ReportResult>(
                finalItems,
                pagedRaw.TotalCount,
                pagedRaw.PageIndex,
                request.PageSize);
        }
    }
}
