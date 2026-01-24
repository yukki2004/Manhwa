using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.ModerateWithStatus
{
    public class ModerateWithStatusRequest
    {
        public short IsPublished { get; set; }
        public string? AdminNote { get; set; }
    }
}
