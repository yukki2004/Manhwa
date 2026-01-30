using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.CreateComment
{
    public class CreateCommentCommand : IRequest<CreateCommentResponse>
    {
        public long UserId { get; set; }
        public long StoryId { get; set; }
        public string UserRole { get; set; } = null!;
        public string Identity { get; set; } = null!;
        public long? ChapterId { get; set; }
        public long? ParentId { get; set; }
        public string Content { get; set; } = null!;
    }
}
