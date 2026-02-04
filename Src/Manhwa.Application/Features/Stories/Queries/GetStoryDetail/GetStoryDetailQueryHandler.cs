using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryDetail
{
    public class GetStoryDetailQueryHandler : IRequestHandler<GetStoryDetailQuery, StoryDetailResponse?>
    {
        private readonly IStoryQueries _storyQueries;

        public GetStoryDetailQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }

        public async Task<StoryDetailResponse?> Handle(GetStoryDetailQuery request, CancellationToken ct)
        {
            return await _storyQueries.GetStoryDetailWithChaptersAsync(request, ct);
        }
    }
}
