using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.LockUser
{
    public class LockUserCommandHandle : IRequestHandler<LockUserCommand, LockUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        public LockUserCommandHandle(IUserRepository userRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<LockUserResponse> Handle(LockUserCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new NotFoundException("user", command.UserId);
            }
            if (user.IsActive == false)
            {
                throw new BusinessRuleViolationException("Tài khoản đang bị khóa", "USER_LOCK");
            }
            user.IsActive = false;
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new SendEmailIntegrationEvent
            {
                To = user.Email,
                Subject = "Thông báo: Tài khoản của bạn đã bị tạm khóa - TruyenVerse",
                TemplateName = "ACCOUNT_LOCKED",
                TemplateData = new Dictionary<string, string>
                {
                    { "Username", user.Username },
                    { "Reason", "Vi phạm quy định cộng đồng hoặc có dấu hiệu bất thường." }
                }
            }, ct);
            var response = new LockUserResponse
            {
                Massage = "Đã khóa tài khoản thành công"
            };
            return response;
        }
    }
}
