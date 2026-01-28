using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Queries.ViewChapter
{
    public class ChapterViewDto
    {
        public long ChapterId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string ChapterSlug { get; init; } = string.Empty;
        public long StoryId { get; init; }
        public string StoryTitle { get; init; } = string.Empty;
        public string StorySlug { get; init; } = string.Empty;
        public List<ImageUrlDto> ImageUrls { get; init; } = new();
    }
}
