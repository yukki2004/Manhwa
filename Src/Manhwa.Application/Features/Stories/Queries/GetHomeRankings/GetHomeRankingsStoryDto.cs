using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeRankings
{
    public class GetHomeRankingsStoryDto
    {
        public long StoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public string Title { get; set; } = null!;
        public int? Release {  get; set; }
    }
}
