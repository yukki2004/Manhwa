using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapterStatus.PublishChapterStatus
{
    public class PublishChapterStatusResponse
    {
        public long ChapterId { get; set; }
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
