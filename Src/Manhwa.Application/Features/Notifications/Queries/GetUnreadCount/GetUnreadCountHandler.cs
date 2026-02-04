using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountHandler : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly INotificationQueries _queries;
        private readonly IUserRepository _userRepository;

        public GetUnreadCountHandler(INotificationQueries queries, IUserRepository userRepository)
        {
            _queries = queries;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null || user.IsActive == false)
            {
                throw new Exception();
            } 

            return await _queries.GetUnreadCountAsync(request.UserId, ct);
        }
    }
}
