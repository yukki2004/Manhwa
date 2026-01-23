using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class ExpConsumer : IConsumer<UserExpActionEvent>
    {
        private readonly IlevelExpRepository _levelExpRepository;
        private readonly IExpLogRepository _expLogRepository;
        private readonly IExpActionRepository _expActionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserRepository _userRepository;
        public ExpConsumer(IlevelExpRepository levelExpRepository,
                           IExpLogRepository expLogRepository,
                           IExpActionRepository expActionRepository,
                           IUnitOfWork unitOfWork,
                           IPublishEndpoint publishEndpoint,
                           IUserRepository userRepository)
        {
            _levelExpRepository = levelExpRepository;
            _expLogRepository = expLogRepository;
            _expActionRepository = expActionRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<UserExpActionEvent> context)
        {
            var msg = context.Message;
            var ct = context.CancellationToken;

            var user = await _userRepository.GetByIdAsync(msg.UserId, ct);
            if (user == null) return;
            if(user.IsActive == false) return;
            var actionConfig = await _expActionRepository.GetByActionTypeAsync(msg.Action, ct);
            if (actionConfig == null) return;

            user.CurrentExp += actionConfig.ExpValue;

            var log = new ExpLog
            {
                UserId = user.UserId,
                Action = msg.Action,
                ExpAmount = actionConfig.ExpValue,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _expLogRepository.AddAsync(log, ct);
            var expRequiredForNextLevel = await _levelExpRepository.GetThresholdForLevelAsync(user.Level + 1, ct);
            if(user.CheckLevelUp(expRequiredForNextLevel))
            {
                var levelData = new
                {
                    newLevel = user.Level
                };
                string metadataJson = JsonSerializer.Serialize(levelData);
                await _publishEndpoint.Publish(new SendNotificationEvent
                {
                    ReceiverIds = new List<long> { user.UserId },
                    Type = Domain.Enums.Notification.NotificationType.LevelUp,
                    RawDataJson = metadataJson, 
                    SenderId = 9 // System
                }, ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }

}

