using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.ChangePublishState
{
    public class ChangePublishStateCommand : IRequest<ChangePublishStateResponse>
    {
        public long StoryId { get; init; }
        public short IsPublished { get; init; }
        public long UserId { get; init; }
    }
}
