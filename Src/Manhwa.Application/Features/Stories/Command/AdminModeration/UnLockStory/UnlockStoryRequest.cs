using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.UnLockStory
{
    public class UnlockStoryRequest
    {
        public string? AdminNote { get; init; }
    }
}
