using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapter
{
    public class UpdateChapterRequest
    {
        public string Title { get; set; } = null!;
        public double ChapterNumber { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
    }
}
