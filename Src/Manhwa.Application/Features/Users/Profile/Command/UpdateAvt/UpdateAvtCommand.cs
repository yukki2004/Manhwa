using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Command.UpdateAvt
{
    public class UpdateAvtCommand : IRequest<bool>
    {
        public long UserId { get; set; }
        public IFormFile File { get; set; } = null!;
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
    }
}
