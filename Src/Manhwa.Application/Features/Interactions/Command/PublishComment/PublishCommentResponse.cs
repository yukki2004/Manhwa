using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.PublishComment
{
    public class PublishCommentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
