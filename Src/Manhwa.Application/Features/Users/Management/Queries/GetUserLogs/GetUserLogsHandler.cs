using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserLogs
{
    public class GetUserLogsHandler : IRequestHandler<GetUserLogsQuery, PagedResult<UserLogResponse>>
    {
        private readonly IUserQueries _userQueries;

        public GetUserLogsHandler(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        public async Task<PagedResult<UserLogResponse>> Handle(GetUserLogsQuery request, CancellationToken ct)
        {
            return await _userQueries.GetPagedUserLogsAsync(
                request.UserId,
                request.PageIndex,
                request.PageSize,
                request.Action,
                ct);
        }
    }
}
