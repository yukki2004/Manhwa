using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser
{
    public class ActiveUserCommand : IRequest<ActiveUserResponse>
    {
        public long UserId { get; set; }
    }
}
