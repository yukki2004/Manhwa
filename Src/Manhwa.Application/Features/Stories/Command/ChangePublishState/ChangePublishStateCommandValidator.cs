using FluentValidation;
using Manhwa.Application.Features.Stories.Command.UpdateStoryStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.ChangePublishState
{
    public class ChangePublishStateCommandValidator : AbstractValidator<ChangePublishStateCommand>
    {
        public ChangePublishStateCommandValidator()
        {
            RuleFor(v => v.StoryId)
                .GreaterThan(0).WithMessage("ID truyện không hợp lệ.");

            RuleFor(v => v.IsPublished)
                .InclusiveBetween((short)0, (short)2)
                .WithMessage("Trạng thái truyện phải là 0 (Delete), 1 (Publish) hoặc 2 (Hide).");

            RuleFor(v => v.UserId)
                .GreaterThan(0).WithMessage("ID người dùng không hợp lệ.");
        }
    }
}
