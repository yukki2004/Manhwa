using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser;
using Manhwa.Domain.Repositories;
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
        public LockUserCommandHandle(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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
            var response = new LockUserResponse();
            return response;
        }
    }
}
