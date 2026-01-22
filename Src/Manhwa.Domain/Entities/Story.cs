using Manhwa.Domain.Enums.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Story
    {
        public long StoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? ReleaseYear { get; set; }
        public string? Author { get; set; }
        public StoryStatus Status { get; set; }
        public bool IsHot { get; set; }
        public StoryPublishStatus IsPublish { get; set; }
        public int TotalView { get; set; }
        public int RateSum { get; set; }
        public int RateCount { get; set; }
        public decimal RateAvg { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long? UserId { get; set; }
        // navigation property
        public User? User { get; set; }
    }
}
