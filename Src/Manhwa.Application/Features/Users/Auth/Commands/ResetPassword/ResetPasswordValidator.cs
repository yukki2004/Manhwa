using FluentValidation;
using Manhwa.Application.Features.Users.Auth.Commands.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator() {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);
            RuleFor(x => x.Otp).NotEmpty();
            RuleFor(x => x.ComfirmPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword).WithMessage("Comfirm password must match new password");
        }
    }
}
