using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeStories
{
    public class GetHomeStoriesQueryHandler : IRequestHandler<GetHomeStoriesQuery, PagedResult<HomeStoryDto>>
    {
        private readonly IStoryQueries _storyQueries;
        public GetHomeStoriesQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }
        public async Task<PagedResult<HomeStoryDto>> Handle(GetHomeStoriesQuery query, CancellationToken ct)
        {
            var response = await _storyQueries.GetPagedHomeStoriesAsync(query.PageIndex, query.PageSize, ct);
            return response;
        }
    }
}
