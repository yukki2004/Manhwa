using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetFavorites
{
    public class GetFavoriteStoriesQueryHandler : IRequestHandler<GetFavoriteStoriesQuery, Manhwa.Application.Common.Abstractions.PagedResult<UserFavoriteDto>>
    {
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepository;
        public GetFavoriteStoriesQueryHandler(IUserQueries userQueries, IUserRepository userRepository)
        {
            _userQueries = userQueries;
            _userRepository = userRepository;
        }
        public async Task<Manhwa.Application.Common.Abstractions.PagedResult<UserFavoriteDto>> Handle(GetFavoriteStoriesQuery query, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId, ct);
            if (user == null || user.IsActive == false)
            {
                throw new NotFoundException("user", query.UserId);
            }
            var response = await _userQueries.GetPagedFavoritesWithChaptersAsync(query.UserId, query.PageIndex, query.PageSize, ct);
            return response;
         
        }
    }
}
