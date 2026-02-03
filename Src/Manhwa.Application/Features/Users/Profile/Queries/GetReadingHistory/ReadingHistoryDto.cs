using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetReadingHistory
{
    public class ReadingHistoryDto
    {
        public long StoryId { get; init; }
        public string StoryTitle { get; init; } = null!;
        public string StorySlug { get; init; } = null!;
        public string? Thumbnail { get; init; }
        public long ChapterId { get; init; }
        public string ChapterTitle { get; init; } = null!;
        public string ChapterSlug { get; init; } = null!;
        public DateTimeOffset LastReadAt { get; init; }
    }
}
