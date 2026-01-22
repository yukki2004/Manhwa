using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<GetAllUsersDto>>
    {
        private readonly IUserQueries _userQueries;
        public GetAllUsersQueryHandler(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }
        public async Task<PagedResult<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            return await _userQueries.GetAllUsersAsync(request, ct);
        }
    }
}
