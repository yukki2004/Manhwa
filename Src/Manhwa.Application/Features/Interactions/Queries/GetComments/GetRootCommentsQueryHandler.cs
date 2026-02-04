using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetComments
{
    public class GetRootCommentsQueryHandler : IRequestHandler<GetRootCommentsQuery, PagedResult<CommentDto>>
    {
        private readonly IInteractionQueries _interactionQueries;

        public GetRootCommentsQueryHandler(IInteractionQueries interactionQueries)
        {
            _interactionQueries = interactionQueries;
        }

        public async Task<PagedResult<CommentDto>> Handle(GetRootCommentsQuery request, CancellationToken ct)
        {
            return await _interactionQueries.GetPagedRootCommentsAsync(request, ct);
        }
    }
}
