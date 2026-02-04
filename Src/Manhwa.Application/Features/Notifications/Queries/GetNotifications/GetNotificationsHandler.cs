using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, PagedResult<NotificationResult>>
    {
        private readonly INotificationQueries _queries;
        private readonly INotificationRepository _repository;
        private readonly IUserRepository _userRepository;

        public GetNotificationsHandler(INotificationQueries queries, INotificationRepository repository, IUserRepository userRepository)
        {
            _queries = queries;
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<NotificationResult>> Handle(GetNotificationsQuery request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null || user.IsActive == false)
            {
                throw new UnauthorizedAccessException("Tài khoản đã bị khóa, không thể nhận thông báo.");
            }
            return await _queries.GetPagedNotificationsAsync(request, ct);
        }
    }
}
