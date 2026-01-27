using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapterStatus.HideChapterStatus
{
    public class HideChapterStatusResponse
    {
        public long ChapterId { get; set; }
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
