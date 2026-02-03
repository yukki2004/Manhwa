using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.Rating
{
    public class RateStoryCommandValidator : AbstractValidator<RateStoryCommand>
    {
        public RateStoryCommandValidator()
        {
            RuleFor(x => x.StoryId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Score).InclusiveBetween((short)1, (short)5)
                .WithMessage("Rating chỉ được trong khoảng từ 1 đến 5 sao thôi bạn nhé!");
        }
    }
}
