using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.DeleteComment
{
    public class DeleteCommentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
