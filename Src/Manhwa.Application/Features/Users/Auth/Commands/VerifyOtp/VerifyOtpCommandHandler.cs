using Manhwa.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, bool>
    {
        private readonly ICacheService _cacheService;
        public VerifyOtpCommandHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"otp:{request.Email}";
            var cachedOtp = await _cacheService.GetAsync<string>(cacheKey);
            if (string.IsNullOrEmpty(cachedOtp))
            {
                return false;
            }
            if (cachedOtp != request.Otp)
            {
                return false;
            }
            return true;
        }
    }
}
