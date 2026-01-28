using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Queries.ViewChapter
{
    public class ViewChapterCommand : IRequest<ChapterViewDto>
    {
        public string Identity { get; set; } = null!;
        public long? UserId { get; set; }
        public string StorySlug { get; set; } = null!;
        public string ChapterSlugs { get; set; } = null!;
        public string? UserRole { get; set; } = null!;
    }
}
