using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.FollowStory
{
    public class FollowStoryResponse
    {
        public bool IsFollowing { get; set; }
        public int FollowCount { get; set; }
        public string Message { get; set; } = null!;
    }
}
