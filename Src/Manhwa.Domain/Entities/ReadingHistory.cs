using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class ReadingHistory
    {
        public long UserId { get; set; }
        public long ChapterId { get; set; }
        public long StoryId { get; set; }
        public DateTimeOffset LastReadAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Story Story { get; set; } = null!;
        public Chapter Chapter { get; set; } = null!;
    }
}
