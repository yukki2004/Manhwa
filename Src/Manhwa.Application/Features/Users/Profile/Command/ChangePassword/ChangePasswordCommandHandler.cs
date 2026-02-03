using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Command.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPublishEndpoint _publishEndpoint;
        public ChangePasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if(user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa không thể đổi mật khẩu");
            }
            if(user.LoginType == LoginType.Google)
            {
                throw new Exception("Không thể đổi mật khẩu khi đăng nhập bằng google");
            }
            if(!_passwordHasher.Verify(command.OldPassword, user.PasswordHash))
            {
                throw new Exception("Mật khẩu cũ không khớp");
            }
            user.PasswordHash = _passwordHasher.Hash(command.NewPassword);
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            // publish event
            await _publishEndpoint.Publish(new UserPasswordResetIntegrationEvent
            {
                UserId = user.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                Action = Domain.Enums.UserLogAction.UpdateProfile,
                CreateAt = DateTimeOffset.UtcNow
            }, cancellationToken);
            await _publishEndpoint.Publish(new SendEmailIntegrationEvent
            {
                To = user.Email,
                Subject = "Cảnh báo bảo mật: Thay đổi mật khẩu",
                TemplateName = "PASSWORD_CHANGED",
                TemplateData = new Dictionary<string, string> {
                                    { "Username", user.Username },
                                    { "IpAddress", command.IpAddress },
                                    { "UserAgent", command.UserAgent }
                }
            });
            return true;
        }

    }
}
