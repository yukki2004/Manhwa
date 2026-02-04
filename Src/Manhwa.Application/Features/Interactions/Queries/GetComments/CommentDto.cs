using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetComments
{
    public class CommentDto
    {
        public long CommentId { get; set; }
        public string Content { get; set; } = null!;
        public DateTimeOffset CreateAt { get; set; }
        public string UserName { get; set; } = null!;
        public string? UserAvatar { get; set; }
        public int Level { get; set; }
        public double? ChapterNumber { get; set; }
        public string? ChapterSlug { get; set; }
        public int ReplyCount { get; set; }
    }
}
