using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Manhwa.Application.Features.Users.Profile.Queries.GetFavorites
{
    public class GetFavoriteStoriesQuery : PagingParamUsers, IRequest<Manhwa.Application.Common.Abstractions.PagedResult<UserFavoriteDto>>
    {
        public long UserId { get; set; }
    }
}
