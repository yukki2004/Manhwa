using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Notifications.Command.MarkAsRead
{
    public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
    {
        private readonly INotificationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsReadHandler(INotificationRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken ct)
        {
            await _repository.MarkAsReadAsync(request.UserId, request.NotificationId, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }
}
