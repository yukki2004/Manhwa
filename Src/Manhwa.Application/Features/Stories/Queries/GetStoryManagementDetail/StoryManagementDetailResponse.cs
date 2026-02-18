using Manhwa.Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryManagementDetail
{
    public class StoryManagementDetailResponse
    {
        public long StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public string? Author { get; set; }
        public int? ReleaseYear { get; set; }

        public string Status { get; set; } = null!; 
        public string PublishStatus { get; set; } = null!;
        public string LockStatus { get; set; } = null!; 

        public int TotalView { get; set; }
        public int FollowCount { get; set; }
        public decimal RateAvg { get; set; }
        public int TotalChapters { get; set; }
        public int FavoriteCount { get; set; }

        public List<string> Genres { get; set; } = new();
        public PagedResult<ChapterManagementItemDto> Chapters { get; set; } = null!;
    }

    public class ChapterManagementItemDto
    {
        public long ChapterId { get; set; }
        public double ChapterNumber { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int TotalView { get; set; }
        public string Status { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
