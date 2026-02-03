using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeStories
{
    public class GetHomeStoriesQuery : PagingParamUsers, IRequest<PagedResult<HomeStoryDto>>
    {
    }
}
