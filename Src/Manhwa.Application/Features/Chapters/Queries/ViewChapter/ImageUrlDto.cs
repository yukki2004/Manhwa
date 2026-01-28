using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Queries.ViewChapter
{
    public class ImageUrlDto
    {
        public string Url { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
