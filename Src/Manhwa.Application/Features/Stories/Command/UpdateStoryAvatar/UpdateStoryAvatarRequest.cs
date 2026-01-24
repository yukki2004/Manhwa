using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryAvatar
{
    public class UpdateStoryAvatarRequest
    {
        public IFormFile? ThumbnailFile { get; set; }
    }
}
