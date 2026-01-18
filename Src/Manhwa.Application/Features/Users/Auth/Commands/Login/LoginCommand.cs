using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Identifier { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
    }
}
