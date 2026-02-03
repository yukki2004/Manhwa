using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetFilteredStories
{
    public class FilterStoriesQueryHandler : IRequestHandler<FilterStoriesQuery, PagedResult<FilteredStoryDto>>
    {
        private readonly IStoryQueries _storyQueries;

        public FilterStoriesQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }

        public async Task<PagedResult<FilteredStoryDto>> Handle(FilterStoriesQuery request, CancellationToken ct)
        {
            return await _storyQueries.GetFilteredStoriesAsync(request, ct);
        }
    }
}
