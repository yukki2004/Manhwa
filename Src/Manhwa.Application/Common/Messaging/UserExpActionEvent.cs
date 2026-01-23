using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class UserExpActionEvent
    {
        public long UserId { get; set; }
        public ExpActionType Action { get; set; }
    }
}
