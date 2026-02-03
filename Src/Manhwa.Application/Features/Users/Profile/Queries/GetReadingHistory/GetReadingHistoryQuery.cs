using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetReadingHistory
{
    public class GetReadingHistoryQuery : PagingParamUsers, IRequest<Application.Common.Abstractions.PagedResult<ReadingHistoryDto>>
    {
        public long UserId { get; set; }
    }
}
