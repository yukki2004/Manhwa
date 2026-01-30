using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class CommentReplyStrategy : INotificationStrategy
    {
        public short Type => 5;

        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");

            var replierName = data.TryGetProperty("replierName", out var rName) ? rName.GetString() : "Người dùng";
            var storyTitle = data.TryGetProperty("storyTitle", out var sTitle) ? sTitle.GetString() : "truyện";
            var slug = data.TryGetProperty("slug", out var sId) ? sId.ToString() : "0";

            return (
                Title: "Phản hồi mới 💬",
                Content: $"{replierName} đã trả lời bình luận của bạn trong truyện \"{storyTitle}\".",
                RedirectUrl: $"/story/{slug}"
            );
        }
    }
}
