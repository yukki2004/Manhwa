using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class StoryInteractionEvent
    {
        public long StoryId { get; init; }
        public long? ChapterId { get; init; }
        public string Identity { get; init; } = string.Empty;
        public InteractionType ActionType { get; init; }
        public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    }
}
