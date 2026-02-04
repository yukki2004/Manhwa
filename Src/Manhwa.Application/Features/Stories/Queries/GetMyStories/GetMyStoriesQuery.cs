using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetMyStories
{
    public class GetMyStoriesQuery : PagingParamUsers, IRequest<PagedResult<MyStoryDto>>
    {
        public long UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
