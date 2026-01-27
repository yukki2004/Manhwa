using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.AddChapter
{
    public class AddChapterRequest
    {
        public string Title { get; set; } = null!;
        public double ChapterNumber { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }
}
