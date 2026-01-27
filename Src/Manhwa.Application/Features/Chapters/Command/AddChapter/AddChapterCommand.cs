using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.AddChapter
{
    public class AddChapterCommand : IRequest<AddChapterResponse>
    {
        public long StoryId { get; set; }
        public long UserId { get; set; }
        public string UserRole { get; set; } = null!;
        public string Title { get; set; } = null!;
        public double ChapterNumber { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }

}
