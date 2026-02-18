using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Stories.Queries.GetFilteredStories;
using Manhwa.Application.Features.Stories.Queries.GetHomeStories;
using Manhwa.Application.Features.Stories.Queries.GetHotStories;
using Manhwa.Application.Features.Stories.Queries.GetMyStories;
using Manhwa.Application.Features.Stories.Queries.GetStoryDetail;
using Manhwa.Application.Features.Stories.Queries.GetStoryManagementDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IStoryQueries
    {
        Task<PagedResult<HomeStoryDto>> GetPagedHomeStoriesAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<PagedResult<FilteredStoryDto>> GetFilteredStoriesAsync(FilterStoriesQuery request, CancellationToken ct);
        Task<StoryDetailResponse?> GetStoryDetailWithChaptersAsync(GetStoryDetailQuery request, CancellationToken ct);
        Task<PagedResult<MyStoryDto>> GetMyStoriesAsync(GetMyStoriesQuery request, CancellationToken ct);
        Task<List<HotStoryDto>> GetAllHotStoriesAsync(CancellationToken ct);
        Task<StoryManagementDetailResponse?> GetStoryManagementDetailAsync(GetStoryManagementDetailQuery request, CancellationToken ct);
    }
}
