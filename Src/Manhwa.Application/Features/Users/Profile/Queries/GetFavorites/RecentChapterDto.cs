using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetFavorites
{
    public class RecentChapterDto
    {
        public long ChapterId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public DateTimeOffset CreateAt { get; set; }
    }
}
