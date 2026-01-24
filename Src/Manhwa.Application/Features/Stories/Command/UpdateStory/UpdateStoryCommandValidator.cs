using FluentValidation;
using System;

namespace Manhwa.Application.Features.Stories.Command.UpdateStory
{
    public class UpdateStoryCommandValidator : AbstractValidator<UpdateStoryCommand>
    {
        public UpdateStoryCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề truyện không được để trống.")
                .MaximumLength(250).WithMessage("Tiêu đề không được vượt quá 250 ký tự.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả truyện không được để trống.")
                .MinimumLength(5).WithMessage("Mô tả truyện nên có ít nhất 20 ký tự để người đọc dễ hiểu.");

            RuleFor(x => x.AuthorName)
                .MaximumLength(100).WithMessage("Tên tác giả không được vượt quá 100 ký tự.");

            RuleFor(x => x.ReleaseYear)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"Năm phát hành phải nằm trong khoảng từ 1900 đến {DateTime.Now.Year + 1}.");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("Truyện phải thuộc ít nhất một thể loại.")
                .Must(ids => ids != null && ids.Count > 0).WithMessage("Danh sách thể loại không hợp lệ.");

            RuleFor(x => x.StoryId)
                .GreaterThan(0).WithMessage("Mã truyện (StoryId) không hợp lệ.");
        }
    }
}