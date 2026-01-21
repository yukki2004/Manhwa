using Manhwa.Application.Common.Extensions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetUserProfile
{
    public class GetUserProfileCommandHandler : IRequestHandler<GetUserProfileCommand, GetUserProfileResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetUserProfileCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GetUserProfileResponse> Handle(GetUserProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(command.Username, cancellationToken);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            if(!user.IsActive)
            {
                throw new Exception("Tài khoản đã bị vô hiệu hóa");
            }
            return new GetUserProfileResponse
            {
                UserId = user.UserId,
                UserName = user.Username,
                Description = user.Description,
                Avatar = user.Avatar.ToFullUrl(),
                Level = user.Level,
                CurrentExp = user.CurrentExp,
            };
        }
    }
}
