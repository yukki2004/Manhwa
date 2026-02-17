using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.MarkStoryHot
{
    public class MarkStoryHotResponse
    {
        public long StoryId { get; set; }
        public bool IsHot { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Message { get; set; } = null!;
    }
}
