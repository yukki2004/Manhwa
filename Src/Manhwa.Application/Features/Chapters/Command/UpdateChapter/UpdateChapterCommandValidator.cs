using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapter
{
    public class UpdateChapterCommandValidator : AbstractValidator<UpdateChapterCommand>
    {
        public UpdateChapterCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ChapterNumber).GreaterThan(0);
        }
    }
}
