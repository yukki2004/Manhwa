using Manhwa.Application.Common.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Notifications.Strategies
{
    public class LevelUpStrategy : INotificationStrategy
    {
        public short Type => 4;

        public (string Title, string Content, string RedirectUrl) Build(string? rawDataJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(rawDataJson ?? "{}");
            var newLevel = data.TryGetProperty("newLevel", out var lv) ? lv.ToString() : "mới";

            return (
                Title: "Thăng cấp thành công! 🎉",
                Content: $"Chúc mừng! Bạn đã đạt đến cấp độ {newLevel}. Tiếp tục đọc truyện để nhận thêm EXP nhé!",
                RedirectUrl: "/profile"
            );
        }
    }
}
