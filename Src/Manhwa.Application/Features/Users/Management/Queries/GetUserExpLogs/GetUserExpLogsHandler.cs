using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserExpLogs
{
    public class GetUserExpLogsHandler : IRequestHandler<GetUserExpLogsQuery, PagedResult<ExpLogResponse>>
    {
        private readonly IUserQueries _userQueries;

        public GetUserExpLogsHandler(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        public async Task<PagedResult<ExpLogResponse>> Handle(GetUserExpLogsQuery request, CancellationToken ct)
        {
            return await _userQueries.GetPagedExpLogsAsync(
                request.UserId,
                request.PageIndex,
                request.PageSize,
                request.Action,
                ct);
        }
    }
}
