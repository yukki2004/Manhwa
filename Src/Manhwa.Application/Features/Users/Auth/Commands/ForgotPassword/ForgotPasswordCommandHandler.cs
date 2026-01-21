using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICacheService _cacheService;
        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint,
            ICacheService cacheService)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            var lockKey = $"lock:forgot-password:{request.Email}";
            var otpKey = $"otp:{request.Email}";
            if (await _cacheService.ExistsAsync(lockKey, ct))
                throw new Exception("Vui lòng đợi 60 giây trước khi yêu cầu mã mới.");
            var user = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng với email đã cung cấp.");
            }
            if(!user.IsActive)
            {
                throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa không thể đặt lại mật khẩu.");
            }
            string otpCode = new Random().Next(0, 1000000).ToString("D6");
            await _cacheService.SetAsync(otpKey, otpCode, TimeSpan.FromMinutes(5), ct);
            await _cacheService.SetAsync(lockKey, "1", TimeSpan.FromSeconds(60), ct);

            await _publishEndpoint.Publish(new SendOtpEmailEvent { Email = request.Email, OtpCode = otpCode}, ct);

            return true;
        }
    }
}
