using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHotStories
{
    public class HotStoryDto
    {
        public long StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public int TotalView { get; set; }
        public decimal RateAvg { get; set; }
        public int FollowCount { get; set; }
    }
}
