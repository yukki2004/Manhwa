using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IReportRepository
    {
        Task AddAsync(Report report, CancellationToken ct = default);
        Task UpdateStatusAsync(long reportId, ReportStatus status, CancellationToken ct = default);
        Task<Report?> GetByIdAsync(long reportId, CancellationToken ct = default);
        Task<bool> IsAlreadyReportedAsync(long userId, long targetId, ReportTargetType targetType, CancellationToken ct = default);
        Task<string?> GetSnapshotAsync(ReportTargetType type, long id, CancellationToken ct);
    }
}
