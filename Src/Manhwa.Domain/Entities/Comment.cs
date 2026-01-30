using Manhwa.Domain.Enums.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Content { get; set; } = null!;
        public CommentStatus Status { get; set; } = CommentStatus.Published;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Foreign Keys
        public long? UserId { get; set; }
        public long StoryId { get; set; }
        public long? ChapterId { get; set; }
        public long? ParentId { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public Story Story { get; set; } = null!;
        public Chapter Chapter { get; set; } = null!;

        // Quan hệ cha - con (Self-referencing)
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
