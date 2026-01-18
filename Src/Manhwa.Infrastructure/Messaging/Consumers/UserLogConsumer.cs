using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using Manhwa.Infrastructure.Persistence;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class UserLogConsumer : IConsumer<UserRegisteredIntegrationEvent>, IConsumer<UserLoggedInIntegrationEvent>
    {
        private readonly IUserLogRepository _userLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserLogConsumer(IUserLogRepository userLogRepository, IUnitOfWork unitOfWork)
        {
            _userLogRepository = userLogRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
        {
            var msg = context.Message;
            await SaveToDatabase(msg.UserId, msg.IpAddress, msg.UserAgent, msg.CreateAt, UserLogAction.Register, context.CancellationToken);
        }
        public async Task Consume(ConsumeContext<UserLoggedInIntegrationEvent> context)
        {
            var msg = context.Message;
            await SaveToDatabase(msg.UserId, msg.IpAddress, msg.UserAgent, msg.CreateAt, UserLogAction.Login, context.CancellationToken);
        }
        private async Task SaveToDatabase(long userId, string ip, string ua, DateTimeOffset createdAt, UserLogAction action, CancellationToken ct)
        {
            var log = new UserLog
            {
                UserId = userId,
                IpAddress = ip,
                UserAgent = ua,
                CreatedAt = createdAt,
                Action = action
            };

            await _userLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
