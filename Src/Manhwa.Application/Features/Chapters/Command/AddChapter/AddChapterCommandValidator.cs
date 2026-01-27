using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.AddChapter
{
    public class AddChapterCommandValidator : AbstractValidator<AddChapterCommand>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
        private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/png", "image/webp", "image/avif" };
        private const long MaxFileSize = 5 * 1024 * 1024; 

        public AddChapterCommandValidator()
        {
            RuleFor(x => x.StoryId)
                .NotEmpty().WithMessage("ID truyện không được để trống.")
                .GreaterThan(0).WithMessage("ID truyện không hợp lệ.");

            RuleFor(x => x.Title)
                .MaximumLength(255).WithMessage("Tiêu đề chương không được quá 255 ký tự.");

            RuleFor(x => x.ChapterNumber)
                .NotNull().WithMessage("Số chương là bắt buộc.")
                .GreaterThanOrEqualTo(0).WithMessage("Số chương không được là số âm.");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("Chương truyện phải có ít nhất một trang ảnh.")
                .Must(x => x.Count <= 200).WithMessage("Một chương không được vượt quá 200 trang ảnh.");

            RuleForEach(x => x.Images).ChildRules(image =>
            {
                image.RuleFor(f => f)
                    .Must(HaveAllowedExtension).WithMessage("Định dạng file không hỗ trợ. Chỉ chấp nhận .jpg, .png, .webp, .avif.")
                    .Must(HaveAllowedMimeType).WithMessage("File gửi lên không phải là ảnh hợp lệ.")
                    .Must(f => f.Length <= MaxFileSize).WithMessage("Mỗi ảnh không được vượt quá 5MB.");
            });
        }
        private bool HaveAllowedExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        private bool HaveAllowedMimeType(IFormFile file)
        {
            return _allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant());
        }
    }
}
