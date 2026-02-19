using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStory
{
    public class UpdateStoryRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? AuthorName { get; set; }
        public int? ReleaseYear { get; set; }
        public List<int>? CategoryIds { get; set; } = new();
    }
}
