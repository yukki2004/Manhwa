using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Interactions.Queries.GetComments;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetReplyComments
{
    public class GetRepliesQueryHandler : IRequestHandler<GetRepliesQuery, PagedResult<CommentReplyDto>>
    {
        private readonly IInteractionQueries _interactionQueries;

        public GetRepliesQueryHandler(IInteractionQueries interactionQueries)
        {
            _interactionQueries = interactionQueries;
        }

        public async Task<PagedResult<CommentReplyDto>> Handle(GetRepliesQuery request, CancellationToken ct)
        {
            return await _interactionQueries.GetPagedRepliesAsync(request, ct);
        }
    }
}
