using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class NewCommentOnStoryStrategy : INotificationStrategy
    {
        public short Type => 6;

        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");

            var commenterName = data.TryGetProperty("commenterName", out var cName) ? cName.GetString() : "Độc giả";
            var storyTitle = data.TryGetProperty("storyTitle", out var sTitle) ? sTitle.GetString() : "tác phẩm của bạn";
            var slug = data.TryGetProperty("slug", out var sId) ? sId.ToString() : "0";

            return (
                Title: "Truyện của bạn có bình luận mới! 📖",
                Content: $"Độc giả **{commenterName}** vừa để lại bình luận trong truyện \"{storyTitle}\". Hãy vào tương tác ngay nhé!",
                RedirectUrl: $"/story/{slug}"
            );
        }
    }
}
