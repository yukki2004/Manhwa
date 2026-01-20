using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Command.UpdateAvt
{
    public class UpdateAvtRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
