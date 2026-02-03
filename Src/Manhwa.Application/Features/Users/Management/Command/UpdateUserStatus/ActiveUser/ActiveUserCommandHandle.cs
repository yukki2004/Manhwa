using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser
{
    public class ActiveUserCommandHandle : IRequestHandler<ActiveUserCommand, ActiveUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ActiveUserCommandHandle(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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
            var response = new ActiveUserResponse();
            return response;
        }
    }
}
