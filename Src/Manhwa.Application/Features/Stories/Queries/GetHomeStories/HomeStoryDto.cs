using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeStories
{
    public class HomeStoryDto
    {

        public long StoryId { get; init; }
        public string Title { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Thumbnail { get; init; }
        public decimal RateAvg { get; init; }
        public int RateCount { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }

        public string? Author { get; init; }
        public string? ShortDescription { get; init; }
        public List<string> Genres { get; init; } = new();

        public List<HomeRecentChapterDto> RecentChapters { get; init; } = new();
    }
}
