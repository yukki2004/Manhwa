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

namespace Manhwa.Application.Features.Users.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPublishEndpoint _publishEndpoint;
        public ResetPasswordCommandHandler(
            ICacheService cacheService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IPublishEndpoint publishEndpoint)
        {
            _cacheService = cacheService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken ct)
        {
            var cacheKey = $"otp:{command.Email}";
            var cachedOtp = await _cacheService.GetAsync<string>(cacheKey);
            if(string.IsNullOrEmpty(cachedOtp) || cachedOtp != command.Otp)
            {
                return false;
            }
            if(command.NewPassword != command.ComfirmPassword)
            {
                return false;
            }
            var user = await _userRepository.GetByEmailAsync(command.Email, ct);
            if(user == null)
            {
                return false;
            }
            user.PasswordHash = _passwordHasher.Hash(command.NewPassword);
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(ct);
            await _cacheService.RemoveAsync(cacheKey);
            await _publishEndpoint.Publish(new UserPasswordResetIntegrationEvent
            {
                UserId = user.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                Action = Domain.Enums.UserLogAction.ResetPassword,
                CreateAt = DateTimeOffset.UtcNow
            }, ct);
            await _publishEndpoint.Publish(new PasswordChangedNotificationEvent
            {
                Email = user.Email,
                Username = user.Username,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
            }, ct);
            return true;
        }
    }
}
