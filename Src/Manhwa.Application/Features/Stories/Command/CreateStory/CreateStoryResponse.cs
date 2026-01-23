using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.CreateStory
{
    public class CreateStoryResponse
    {
        public long StoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
