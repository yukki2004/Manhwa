using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.UpdateComment
{
    public class UpdateCommentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
