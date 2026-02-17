using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.UnmarkStoryHot
{
    public class UnmarkStoryHotCommand : IRequest<UnmarkStoryHotResponse>
    {
        public long StoryId { get; set; }
    }
}
