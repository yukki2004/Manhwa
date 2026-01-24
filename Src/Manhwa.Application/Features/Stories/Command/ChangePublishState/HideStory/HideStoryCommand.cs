using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.ChangePublishState.HideStory
{
    public class HideStoryCommand : IRequest<bool>
    {
        public long StoryId { get; init; }
        public long UserId { get; init; }
        public string UserRole { get; init; } = null!;
    }
}
