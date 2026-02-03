using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser
{
    public class ActiveUserCommandHandle : IRequestHandler<ActiveUserCommand, ActiveUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        public ActiveUserCommandHandle(IUserRepository userRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<ActiveUserResponse> Handle(ActiveUserCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new NotFoundException("user", command.UserId);
            }
            if(user.IsActive == true)
            {
                throw new BusinessRuleViolationException("Tài khoản đang hoạt động", "USER_ACTIVE");
            }
            user.IsActive = true;
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new SendEmailIntegrationEvent
            {
                To = user.Email,
                Subject = "Chào mừng trở lại! Tài khoản của bạn đã được kích hoạt - TruyenVerse",
                TemplateName = "ACCOUNT_ACTIVATED",
                TemplateData = new Dictionary<string, string>
                {
                    { "Username", user.Username }
                }
            }, ct);
            var response = new ActiveUserResponse
            {
                Massage = "Đã mở khóa tài khoản"
            };
            return response;
        }
    }
}
