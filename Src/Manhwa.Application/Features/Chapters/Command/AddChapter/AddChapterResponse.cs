using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.AddChapter
{
    public class AddChapterResponse
    {
        public long ChapterId { get; set; }
        public double ChapterNumber { get; set; }
        public string ChapterSlug { get; set; } = null!;
        public string StorySlug { get; set; } = null!;
        public string Message { get; set; } = "Chương mới đã được đăng tải thành công!";
        public string FullUrl => $"/truyen/{StorySlug}/{ChapterSlug}";
    }
}
