using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStory
{
    public class UpdateStoryCommand : IRequest<UpdateStoryResponse>
    {
        public long StoryId { get; set; }
        public long UserId { get; set; }
        public string UserRole { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? AuthorName { get; set; }
        public int? ReleaseYear { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}
