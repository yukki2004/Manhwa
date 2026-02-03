using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Stories.Queries.GetHomeStories;
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
    }
}
