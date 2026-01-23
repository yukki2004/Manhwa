using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.CreateStory
{
    public class CreateStoryValidator : AbstractValidator<CreateStoryCommand>
    {
        public CreateStoryValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(5);
            RuleFor(x => x.GenreIds).NotEmpty().WithMessage("Truyện phải có ít nhất một thể loại.");
            RuleFor(x => x.UserId).GreaterThan(0);

            if (DateTimeOffset.UtcNow.Year > 0)
                RuleFor(x => x.ReleaseYear).InclusiveBetween(1900, DateTime.Now.Year + 1);
        }
    }
}
