using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus
{
    public class UpdateUserStatusCommand : IRequest<bool>
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
    }
}
