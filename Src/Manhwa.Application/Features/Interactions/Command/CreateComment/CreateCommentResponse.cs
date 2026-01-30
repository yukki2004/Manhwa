using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.CreateComment
{
    public class CreateCommentResponse
    {
        public long CommentId { get; set; }
        public string Message { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
