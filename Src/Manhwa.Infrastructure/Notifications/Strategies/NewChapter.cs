using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class NewChapterStrategy : INotificationStrategy
    {
        public short Type => 1;

        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");

            string storyTitle = data.TryGetProperty("StoryTitle", out var sTitle) ? sTitle.GetString() : "Truyện bạn theo dõi";
            string chapterNumber = data.TryGetProperty("ChapterNumber", out var cNum) ? cNum.ToString() : "";
            string storySlug = data.TryGetProperty("StorySlug", out var sSlug) ? sSlug.GetString() : "";
            string chapterSlug = data.TryGetProperty("ChapterSlug", out var cSlug) ? cSlug.GetString() : "";

            return (
                Title: $"🔥 Chương mới: {storyTitle}",
                Content: $"Chương {chapterNumber} vừa được đăng tải. Đọc ngay kẻo lỡ!",
                RedirectUrl: $"/truyen/{storySlug}/{chapterSlug}"
            );
        }
    }
}
