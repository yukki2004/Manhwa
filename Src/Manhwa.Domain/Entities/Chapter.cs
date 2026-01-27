using Manhwa.Domain.Enums.Chapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Chapter
    {
        public long ChapterId { get; set; }
        public string? Title { get; set; }
        public string Slug { get; set; } = null!; 
        public double ChapterNumber { get; set; }
        public int TotalView { get; set; }
        public ChapterStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Foreign Key & Navigation Property
        public long StoryId { get; set; }
        public Story Story { get; set; } = null!;
        public ICollection<ChapterImage> ChapterImages { get; set; } = new List<ChapterImage>();
        public ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
