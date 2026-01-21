using Manhwa.Application.Common.Extensions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetMyProfile
{
    public class GetMyProfileCommandHandler : IRequestHandler<GetMyProfileCommand, GetMyProfileResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetMyProfileCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GetMyProfileResponse> Handle(GetMyProfileCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if(user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            if (!user.IsActive)
            {
                throw new Exception("Tài khoản của bạn đã bị khóa");
            }
            var userResponse = new GetMyProfileResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar.ToFullUrl(),
                Description = user.Description,
                Level = user.Level,
                CurrentExp = user.CurrentExp,
                LoginType = user.LoginType,
                Role = user.Role,
                CreateAt = user.CreatedAt,
                IsActive = user.IsActive

            };
            return userResponse;
        }
    }
}
