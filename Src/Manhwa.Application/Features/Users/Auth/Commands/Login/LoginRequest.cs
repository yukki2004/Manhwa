using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.Login
{
    public class LoginRequest
    {
        public string Identifier { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
