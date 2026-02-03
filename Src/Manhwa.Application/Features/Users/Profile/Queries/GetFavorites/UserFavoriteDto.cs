using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetFavorites
{
    public class UserFavoriteDto
    {
        public long StoryId { get; init; }
        public string Title { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Thumbnail { get; init; }
        public List<RecentChapterDto> RecentChapters { get; init; } = new();
    }

}
