using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using Manhwa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserExpLogs
{
    public class GetUserExpLogsQuery : PagingParamUsers, IRequest<PagedResult<ExpLogResponse>>
    {
        public long UserId { get; set; }
        public ExpActionType? Action { get; set; }

    }
}
