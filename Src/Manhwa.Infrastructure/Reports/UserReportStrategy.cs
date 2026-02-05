using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Report;
using Manhwa.Domain.Enums.Report;
using Manhwa.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Reports
{
    public class UserReportStrategy : IReportTargetStrategy
    {
        private readonly AppDbContext _context;
        public UserReportStrategy(AppDbContext context) => _context = context;
        public ReportTargetType TargetType => ReportTargetType.User;

        public async Task<(string TargetName, string RedirectUrl, string? AvatarUrl)> ResolveAsync(long targetId, CancellationToken ct)
        {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.UserId == targetId)
                .Select(u => new { u.Username, u.Avatar })
                .FirstOrDefaultAsync(ct);

            return (user?.Username ?? "N/A", $"/user/{user?.Username}", user?.Avatar.ToFullUrl());
        }
    }
}
