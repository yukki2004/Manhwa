using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryManagementDetail
{
    public class GetStoryManagementDetailQuery : PagingParamUsers, IRequest<StoryManagementDetailResponse?>
    {
        public long StoryId { get; set; }
        public bool IsAdmin { get; set; } 
    }
}
