using Manhwa.Application.Common.Abstractions;
using Manhwa.Domain.Enums.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryDetail
{
    public class StoryDetailResponse
    {
        public long StoryId { get; init; }
        public string Title { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public int? Realease_year { get; set; }
        public string? Description { get; init; }
        public string? Author { get; init; }
        public string? Thumbnail { get; init; }
        public StoryStatus Status { get; set; } 
        public int TotalView { get; set; }
        public decimal RateAvg { get; init; }
        public int RateCount { get; init; }
        public int FavoriteCount { get; init; } 
        public int TotalChapters { get; init; } 
        public DateTimeOffset CreateAt { get; set; }
        public List<CategoryStoryDetailDto> Genres { get; init; } = new();
        public PagedResult<ChapterItemDto> Chapters { get; init; } = null!;
    }
}
