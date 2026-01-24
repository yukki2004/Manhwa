using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryStatus
{
    public class UpdateStoryStatusCommand : IRequest<UpdateStoryStatusResponse>
    {
        public long StoryId { get; init; }
        public short Status { get; init; }
        public long UserId { get; init; }
        
    }
}
