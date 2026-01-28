using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Interfaces.Ranking;
using Manhwa.Application.Common.Messaging;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class StoryInteractionConsumer : IConsumer<StoryInteractionEvent>
    {
        private readonly IEnumerable<IInteractionStrategy> _interactionStrategies;
        private readonly IUnitOfWork _unitOfWork;
        public StoryInteractionConsumer(IEnumerable<IInteractionStrategy> interactionStrategies, IUnitOfWork unitOfWork)
        {
            _interactionStrategies = interactionStrategies;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<StoryInteractionEvent> context)
        {
            var message = context.Message;
            var strategy = _interactionStrategies.FirstOrDefault(s => s.Type == message.ActionType);
            if (strategy != null)
            {
                await strategy.HandleAsync(message.StoryId, message.ChapterId, message.Identity);
            }
        }

    }
}
