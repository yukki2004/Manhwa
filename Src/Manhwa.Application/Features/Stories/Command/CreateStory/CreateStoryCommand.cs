using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.CreateStory
{
    public class CreateStoryCommand : IRequest<CreateStoryResponse>
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public IFormFile? ThumbnailFile { get; init; } 
        public int? ReleaseYear { get; init; }
        public string? Author { get; init; }
        public long UserId { get; init; }
        public List<int> GenreIds { get; init; } = new(); 
    }
}
