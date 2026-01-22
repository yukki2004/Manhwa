using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class ChapterImage
    {
        public long ChapterImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int OrderIndex { get; set; }

        // Foreign Key & Navigation Property
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; } = null!;
    }
}
