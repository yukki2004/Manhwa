using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterCommandHandler(IPublishEndpoint publishEndpoint, IPasswordHasher passwordHasher, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken ct) 
        {
            var userName = command.Username;
            var email = command.Email;

            var userNameCheck = await _userRepository.ExistsByUsernameAsync(userName, ct);
            var emailCheck = await _userRepository.ExistsByEmailAsync(email, ct);

            if (userNameCheck || emailCheck)
            {
                return new RegisterResponse
                {
                    Message = "Username hoặc email đã tồn tại"
                };
            }
            var passwordHash = _passwordHasher.Hash(command.Password);
            var newUser = new User
            {
                Username = userName,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };
            await _userRepository.AddAsync(newUser, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new UserRegisteredIntegrationEvent
            {
                UserId = newUser.UserId,
                UserAgent = command.UserAgent,
                IpAddress = command.IpAddress,
                CreateAt = DateTimeOffset.UtcNow,
            }, ct);
            await _publishEndpoint.Publish(new SendEmailIntegrationEvent
            {
                To = newUser.Email,
                Subject = "Chào mừng bạn gia nhập thế giới TruyenVerse!",
                TemplateName = "WELCOME_NEW_USER",
                TemplateData = new Dictionary<string, string>
                {
                    { "Username", newUser.Username }
                }
            }, ct);
            return new RegisterResponse
            {
                Message = "Đăng ký thành công"
            };
        }
    }
}
