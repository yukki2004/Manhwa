using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class RefreshToken
    {
        public long RefreshTokenId { get; set; }
        public string Token { get; set; } = null!;
        public bool IsUsed { get; set; } = false;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long UserId { get; set; }
        // Navigation property
        public User User { get; set; } = null!;

    }
}
