using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetAllUsers
{
    public class GetAllUsersQuery : PagingParamUsers, IRequest<PagedResult<GetAllUsersDto>>
    {
        public string? SearchTerm { get; init; }
        public bool? IsActive { get; init; }
    }
}
