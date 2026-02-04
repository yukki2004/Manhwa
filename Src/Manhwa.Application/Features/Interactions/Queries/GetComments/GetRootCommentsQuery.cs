using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Queries.GetComments
{
    public class GetRootCommentsQuery : PagingParamUsers, IRequest<PagedResult<CommentDto>>
    {
        public string StorySlug { get; set; } = null!;
        public string? ChapterSlug { get; set; }    
    }
}
