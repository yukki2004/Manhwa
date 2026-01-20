using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Command.UpdateAvt
{
    public class UpdateAvtCommandValidator : AbstractValidator<UpdateAvtCommand>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long _maxFileSize = 2 * 1024 * 1024;

        public UpdateAvtCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Vui lòng chọn ảnh.")
                .Must(file => file.Length <= _maxFileSize).WithMessage("Dung lượng ảnh không được vượt quá 2MB.")
                .Must(file => _allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Định dạng file không hỗ trợ (Chỉ chấp nhận .jpg, .png, .webp).");
        }
    }
}
