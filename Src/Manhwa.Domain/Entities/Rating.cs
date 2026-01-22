using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Rating
    {
        public long StoryId { get; set; }
        public long UserId { get; set; }
        public short Score { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation properties
        public Story Story { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
