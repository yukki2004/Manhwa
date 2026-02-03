using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.LockUser
{
    public class LockUserCommand : IRequest<LockUserResponse>
    {
        public long UserId { get; set; }
    }
}
