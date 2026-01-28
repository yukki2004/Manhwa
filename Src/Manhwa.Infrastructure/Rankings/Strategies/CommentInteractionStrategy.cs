using Manhwa.Application.Common.Interfaces.Ranking;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Rankings.Strategies
{
    public class CommentInteractionStrategy : BaseInteractionStrategy, IInteractionStrategy
    {
        public InteractionType Type => InteractionType.Comment;

        public CommentInteractionStrategy(ICacheService cache) : base(cache) { }

        public async Task HandleAsync(long storyId, long? chapterId, string identity)
        {
            await UpdateZSetRanking(storyId, 2);
        }
    }
}
