using FluentValidation;
using Manhwa.Application.Features.Stories.Command.ChangePublishState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.ModerateWithStatus
{
    public class ModerateWithStatusCommandValidator : AbstractValidator<ModerateWithStatusCommand>
    {
        public ModerateWithStatusCommandValidator()
        {
            RuleFor(v => v.StoryId)
    .GreaterThan(0).WithMessage("ID truyện không hợp lệ.");

            RuleFor(v => v.IsPublished)
                .InclusiveBetween((short)0, (short)2)
                .WithMessage("Trạng thái truyện phải là 0 (Delete), 1 (Publish) hoặc 2 (Hide).");
        }
    }
}
