using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetReplyComments
{
    public class CommentReplyDto
    {
        public long CommentId { get; set; }
        public string Content { get; set; } = null!;
        public int ReplyCount { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public long? ParentId { get; set; }
        public string UserName { get; set; } = null!;
        public string? UserAvatar { get; set; }
        public int Level { get; set; }

        public string? RepliedToUserName { get; set; }
    }
}
