using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.UpdateComment
{
    public class UpdateCommentCommand : IRequest<UpdateCommentResponse>
    {
        public long CommentId { get; set; }
        public long UserId { get; set; }
        public string UserRole { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
