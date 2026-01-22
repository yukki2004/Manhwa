using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class UserFavorite
    {
        public long StoryId { get; set; }
        public long UserId { get; set; }

        // Navigation properties
        public Story Story { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
