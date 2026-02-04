using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using Manhwa.Application.Features.Interactions.Queries.GetComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetReplyComments
{
    public class GetRepliesQuery : PagingParamUsers, IRequest<PagedResult<CommentReplyDto>>
    {
        public long ParentId { get; set; }
    }
}
