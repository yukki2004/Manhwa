using Manhwa.Domain.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Report
{
    public interface IReportTargetStrategy
    {
        ReportTargetType TargetType { get; }
        Task<(string TargetName, string RedirectUrl, string? AvatarUrl)> ResolveAsync(long targetId, CancellationToken ct);
    }
}
