using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Reports.Queries.GetReport;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class ReportQueries : IReportQueries
    {
        private readonly AppDbContext _context;

        public ReportQueries(AppDbContext context) => _context = context;

        public async Task<PagedResult<ReportRawDto>> GetPagedReportsAsync(GetReportsQuery request, CancellationToken ct)
        {
            var query = _context.Reports
                .AsNoTracking()
                .Include(r => r.User) 
                .OrderByDescending(r => r.CreateAt)
                .AsQueryable();
            if (request.Status.HasValue)
                query = query.Where(r => r.Status == request.Status.Value);

            if (request.TargetType.HasValue)
                query = query.Where(r => r.TargetType == request.TargetType.Value);

            var projection = query.Select(r => new ReportRawDto
            {
                ReportId = r.ReportId,
                Reason = r.Reason,
                Status = r.Status,
                CreateAt = r.CreateAt,
                TargetId = r.TargetId,
                TargetType = r.TargetType,
                ReporterName = r.User.Username
            });

            return await projection.ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }
    }
}
