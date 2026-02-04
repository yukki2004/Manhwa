using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Command.MarkAllAsRead
{
    public class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand, bool>
    {
        private readonly INotificationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public MarkAllAsReadHandler(INotificationRepository repository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(MarkAllAsReadCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null || user.IsActive == false)
            {
                throw new Exception();
            }
            await _repository.MarkAllAsReadAsync(request.UserId, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }
}
