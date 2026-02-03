using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeStories
{
    public class HomeRecentChapterDto
    {
        public long ChapterId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public DateTimeOffset CreateAt { get; set; }
    }
}
