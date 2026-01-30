using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Nội dung bình luận không được để trống.")
                .MaximumLength(1500).WithMessage("Bình luận không được vượt quá 1500 ký tự.");

            RuleFor(x => x.StoryId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
