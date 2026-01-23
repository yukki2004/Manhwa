using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Abstractions
{
    public class NotificationResult
    {
        public long NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? RedirectUrl { get; set; } 
        public string? Metadata { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsRead { get; set; }

        // Thông tin hiển thị "Vỏ bọc"
        public string SenderName { get; set; } = "Hệ thống TruyenVerse";
        public string SenderAvatar { get; set; } = "/assets/images/system-avatar.png";
    }
}
