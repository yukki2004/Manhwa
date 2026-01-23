using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class NewStoryStrategy : INotificationStrategy
    {
        public short Type => 7;

        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");

            string storyTitle = data.TryGetProperty("title", out var t) ? t.ToString() : "Một truyện mới";
            string slug = data.TryGetProperty("slug", out var s) ? s.ToString() : "";

            return (
                Title: "🆕 Truyện mới cập bến!",
                Content: $"Siêu phẩm '{storyTitle}' vừa được đăng tải. Khám phá ngay!",
                RedirectUrl: $"/story/{slug}"
            );
        }
    }
}
