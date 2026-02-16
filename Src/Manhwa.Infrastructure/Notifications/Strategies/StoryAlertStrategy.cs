using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class StoryAlertStrategy : INotificationStrategy
    {
        public short Type => 3;
        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");
            string storyTitle = data.TryGetProperty("title", out var t) ? t.ToString() : "Một truyện mới";
            string adminNote = data.TryGetProperty("adminNote", out var n) ? n.ToString() : "";
            string slug = data.TryGetProperty("slug", out var s) ? s.ToString() : "";
            string reason = data.TryGetProperty("reason", out var r) ? r.ToString() : "Không rõ lí do";

            return (
                Title: "Thông báo!",
                Content: $"Truyện của bạn '{storyTitle}' đã {reason} với lí do: {adminNote}",
                RedirectUrl: $"/truyen/{slug}"
            );
        }

    }
}
