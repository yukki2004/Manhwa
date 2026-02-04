using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Interactions.Queries.GetComments;
using Manhwa.Application.Features.Interactions.Queries.GetReplyComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IInteractionQueries
    {
        Task<PagedResult<CommentDto>> GetPagedRootCommentsAsync(GetRootCommentsQuery request, CancellationToken ct);
        Task<PagedResult<CommentReplyDto>> GetPagedRepliesAsync(GetRepliesQuery request, CancellationToken ct);
    }
}
