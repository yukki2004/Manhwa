using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.MarkStoryHot
{
    public class MarkStoryHotCommand : IRequest<MarkStoryHotResponse>
    {
        public long StoryId { get; set; }
    }
}
