using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Ranking
{
    public interface IInteractionStrategy
    {
        InteractionType Type { get; }
        Task HandleAsync(long storyId, long? chapterId, string identity);
    }
}
