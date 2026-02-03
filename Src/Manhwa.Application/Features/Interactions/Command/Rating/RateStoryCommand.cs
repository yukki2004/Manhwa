using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.Rating
{
    public class RateStoryCommand : IRequest<RateStoryResponse>
    {
        public long UserId { get; set; }
        public string UserRole { get; set; } = null!;
        public long StoryId { get; set; }
        public short Score { get; set; }
    }
}
