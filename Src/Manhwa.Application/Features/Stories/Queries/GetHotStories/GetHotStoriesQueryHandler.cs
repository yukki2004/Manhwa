using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHotStories
{
    public class GetHotStoriesQueryHandler : IRequestHandler<GetHotStoriesQuery, List<HotStoryDto>>
    {
        private readonly IStoryQueries _storyQueries;

        public GetHotStoriesQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }

        public async Task<List<HotStoryDto>> Handle(GetHotStoriesQuery request, CancellationToken ct)
        {
            return await _storyQueries.GetAllHotStoriesAsync(ct);
        }
    }
}
