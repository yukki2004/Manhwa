using Manhwa.Domain.Enums;
using Manhwa.Domain.Enums.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetMyStories
{
    public class MyStoryDto
    {
        public long StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public StoryPublishStatus Status { get; set; }

        public AdminLockStatus AdminLockStatus { get; set; }
        public string? AdminNote { get; set; }
        public decimal RateAvg { get; init; }
        public int RateCount { get; init; }
        public int TotalView { get; set; }
        public int FavoriteCount { get; set; } 
        public DateTimeOffset CreateAt { get; set; } 
    }
}
