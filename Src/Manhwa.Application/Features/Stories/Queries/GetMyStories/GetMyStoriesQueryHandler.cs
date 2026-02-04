using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetMyStories
{
    public class GetMyStoriesQueryHandler : IRequestHandler<GetMyStoriesQuery, PagedResult<MyStoryDto>>
    {
        private readonly IStoryQueries _storyQueries;

        public GetMyStoriesQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }

        public async Task<PagedResult<MyStoryDto>> Handle(GetMyStoriesQuery request, CancellationToken ct)
        {
            return await _storyQueries.GetMyStoriesAsync(request, ct);
        }
    }
}
