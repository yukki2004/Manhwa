using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryManagementDetail
{
    public class GetStoryManagementDetailQueryHandler : IRequestHandler<GetStoryManagementDetailQuery, StoryManagementDetailResponse?>
    {
        private readonly IStoryQueries _storyQueries;

        public GetStoryManagementDetailQueryHandler(IStoryQueries storyQueries)
        {
            _storyQueries = storyQueries;
        }

        public async Task<StoryManagementDetailResponse?> Handle(GetStoryManagementDetailQuery request, CancellationToken ct)
        {
            return await _storyQueries.GetStoryManagementDetailAsync(request, ct);
        }
    }
}
