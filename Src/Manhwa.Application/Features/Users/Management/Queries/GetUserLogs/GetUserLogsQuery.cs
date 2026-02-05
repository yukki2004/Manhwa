using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using Manhwa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserLogs
{
    public class GetUserLogsQuery : PagingParamUsers, IRequest<PagedResult<UserLogResponse>>
    {
        public long UserId { get; set; }
        public UserLogAction? Action { get; set; }
    }
}
