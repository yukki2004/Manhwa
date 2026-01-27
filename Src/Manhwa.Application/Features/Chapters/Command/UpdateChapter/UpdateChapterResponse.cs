using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapter
{
    public class UpdateChapterResponse
    {
        public long ChapterId { get; set; }
        public string Message { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
