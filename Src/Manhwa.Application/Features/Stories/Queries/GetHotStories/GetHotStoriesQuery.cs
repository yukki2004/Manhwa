using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHotStories
{
    public class GetHotStoriesQuery : IRequest<List<HotStoryDto>>
    {

    }
}
