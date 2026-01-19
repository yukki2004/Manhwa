using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.ResetPassword
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string ComfirmPassword { get; set; } = null!;
    }
}
