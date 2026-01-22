using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Users.Management.Queries.GetAllUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<PagedResult<GetAllUsersDto>> GetAllUsersAsync(GetAllUsersQuery request, CancellationToken ct);
    }
}
