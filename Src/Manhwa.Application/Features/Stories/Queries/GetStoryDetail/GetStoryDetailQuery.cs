using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryDetail
{
    public class GetStoryDetailQuery : PagingParamUsers, IRequest<StoryDetailResponse?>
    {
        public string Slug { get; set; } = null!;
    }
}
