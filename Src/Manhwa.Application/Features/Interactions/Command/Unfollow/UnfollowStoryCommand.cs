using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.Unfollow
{
    public class UnfollowStoryCommand : IRequest<UnfollowStoryResponse>
    {
        public long StoryId { get; set; }
        public long UserId { get; set; }
    }
}
