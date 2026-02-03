using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetFilteredStories
{
    public class FilteredStoryDto
    {
        public long StoryId { get; init; }
        public string Title { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Thumbnail { get; init; }
        public decimal RateAvg { get; init; }
        public string? Author { get; init; }
        public string? ShortDescription { get; init; }
        public List<string> Genres { get; init; } = new();
        public List<FilteredRecentChapterDto> RecentChapters { get; init; } = new();
    }
}
