using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.LockStory
{
    public class LockStoryCommand : IRequest<bool>
    {
        public long StoryId { get; init; }
        public string? AdminNote { get; init; }
    }
}
