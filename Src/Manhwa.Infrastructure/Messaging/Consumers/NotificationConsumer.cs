using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Interfaces.Notifications;
using Manhwa.Application.Common.Mappings;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class NotificationConsumer : IConsumer<SendNotificationEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRealtimeService _realtimeService;
        private readonly IEnumerable<INotificationStrategy> _strategies;
        public NotificationConsumer(INotificationRepository notificationRepository,
                                    IUnitOfWork unitOfWork,
                                    IRealtimeService realtimeService, 
                                    IEnumerable<INotificationStrategy> strategies)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _realtimeService = realtimeService;
            _strategies = strategies;
        }
        public async Task Consume(ConsumeContext<SendNotificationEvent> context)
        {
            var msg = context.Message;
            var strategy = _strategies.FirstOrDefault(s => s.Type == (short)msg.Type);
            if (strategy == null) return;
            var (title, content, redirectUrl) = strategy.Build(msg.RawDataJson);
            var notification = new Domain.Entities.Notification
            {
                Title = title,
                Content = content,
                Type = msg.Type,
                Metadata = msg.RawDataJson,
                RedirectUrl = redirectUrl,
                SenderId = msg.SenderId
            };
            await _notificationRepository.AddAsync(notification, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            User? senderInfo = null;
            if (msg.SenderId.HasValue)
            {
                senderInfo = await _notificationRepository.GetSenderAsync(msg.SenderId.Value, context.CancellationToken);
            }
            var userNotifs = msg.ReceiverIds.Select(userId => new UserNotification
            {
                UserId = userId,
                NotificationId = notification.NotificationId,
                IsRead = false,
                ReadAt = DateTimeOffset.UtcNow
            }).ToList();
            await _notificationRepository.AddUserNotificationsAsync(userNotifs, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            notification.Sender = senderInfo;
            var resultDto = NotificationMapping.MapToResult(notification, false);
            foreach (var userId in msg.ReceiverIds)
            {
                await _realtimeService.SendToUserAsync(userId, "ReceiveNotification", resultDto);
            }
        }
    }
}
