using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Manhwa.Domain.Entities
{
    public class User
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Description { get; set; }
        public string? PasswordHash { get; set; }
        public string? Avatar { get; set; }
        public LoginType LoginType { get; set; }
        public string? GoogleId { get; set; }
        public UserRole? Role { get; set; }
        public int CurrentExp { get; set; } = 0;
        public short Level { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        // Navigation properties
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
        public ICollection<ExpLog> ExpLogs { get; set; } = new List<ExpLog>();
        public ICollection<Story> stories { get; set; } = new List<Story>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
        public ICollection<Report> Reports { get; set; }= new List<Report>();
        public bool CheckLevelUp(int totalExpRequiredForNextLevel)
        {
            if (this.CurrentExp >= totalExpRequiredForNextLevel)
            {
                this.Level++;
                return true;
            }
            return false;
        }
    }
}
