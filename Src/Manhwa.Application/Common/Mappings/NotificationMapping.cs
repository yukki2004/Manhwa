using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Mappings
{
    public static class NotificationMapping
    {
        public static NotificationResult MapToResult(Notification notification, bool isRead)
        {
            var result = new NotificationResult
            {
                NotificationId = notification.NotificationId,
                Title = notification.Title,
                Content = notification.Content,
                Type = notification.Type.ToString(),
                RedirectUrl = notification.RedirectUrl,
                Metadata = notification.Metadata,
                CreatedAt = notification.CreatedAt,
                IsRead = isRead
            };

            if (notification.Sender != null)
            {
                result.SenderName = notification.Sender.Username;
                result.SenderAvatar = $"{notification.Sender.Avatar.ToFullUrl()}?v={(notification.Sender.UpdatedAt).Ticks}";
            }
            else
            {
                result.SenderName = "Hệ thống TruyenVerse";
                result.SenderAvatar.ToFullUrl();
            }

            return result;
        }
    }
}
