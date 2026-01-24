using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.ModerateWithStatus
{
    public class ModerateWithStatusCommand : IRequest<ModerateWithStatusResponse>
    {
        public long StoryId { get; init; }
        public short IsPublished { get; init; }
        public string? AdminNote { get; init; }
    }
}
