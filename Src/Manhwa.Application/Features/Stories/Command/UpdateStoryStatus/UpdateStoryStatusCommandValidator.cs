using FluentValidation;
using Manhwa.Application.Features.Users.Auth.Commands.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryStatus
{
    public class UpdateStoryStatusCommandValidator : AbstractValidator<UpdateStoryStatusCommand>
    {
        public UpdateStoryStatusCommandValidator()
        {
            RuleFor(v => v.StoryId)
                .GreaterThan(0).WithMessage("ID truyện không hợp lệ.");

            RuleFor(v => v.Status)
                .InclusiveBetween((short)0, (short)2)
                .WithMessage("Trạng thái truyện phải là 0 (Ongoing), 1 (Complete) hoặc 2 (Dropped).");

            RuleFor(v => v.UserId)
                .GreaterThan(0).WithMessage("ID người dùng không hợp lệ.");
        }
    }
}
