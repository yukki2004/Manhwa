using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapter
{
    public class UpdateChapterCommandValidator : AbstractValidator<UpdateChapterCommand>
    {
        public UpdateChapterCommandValidator()
        {
            RuleFor(x => x.ChapterId)
                .NotEmpty().WithMessage("ChapterId không được để trống.")
                .GreaterThan(0).WithMessage("ChapterId không hợp lệ.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề chương không được để trống.")
                .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

            RuleFor(x => x.ChapterNumber)
                .GreaterThan(0).WithMessage("Số chương phải lớn hơn 0.");

            RuleForEach(x => x.Images)
                .SetValidator(new FileValidator());
        }
    }

    public class FileValidator : AbstractValidator<IFormFile>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; 

        public FileValidator()
        {
            RuleFor(x => x.Length)
                .NotNull()
                .LessThanOrEqualTo(MaxFileSize)
                .WithMessage(x => $"File {x.FileName} quá lớn. Tối đa 5MB.");

            RuleFor(x => x.FileName)
                .Must(HaveAllowedExtension)
                .WithMessage(x => $"File {x.FileName} không đúng định dạng. Chỉ nhận .jpg, .png, .webp");
        }

        private bool HaveAllowedExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }
    }
}