using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetFilteredStories
{
    public class FilterStoriesQuery : PagingParamUsers, IRequest<PagedResult<FilteredStoryDto>>
    {
        public string? SearchTerm { get; init; } // Tìm theo tên hoặc slug
        public List<string>? CategorySlugs { get; init; }
        public int? ReleaseYear { get; init; } // Lọc theo năm
        public int? MinChapters { get; init; } // Lọc truyện có số chương >= X
        public string? SortBy { get; init; } // latest, views, rating
    }
}
