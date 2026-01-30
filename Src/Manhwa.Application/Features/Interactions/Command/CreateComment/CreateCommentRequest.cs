using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.CreateComment
{
    public class CreateCommentRequest
    {
        public long StoryId { get; set; }
        public long? ChapterId { get; set; }
        public long? ParentId { get; set; }
        public string Content { get; set; } = null!;
    }
}
