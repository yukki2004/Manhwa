using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryAvatar
{
    public class UpdateStoryAvatarCommand : IRequest<UpdateStoryAvatarResponse>
    {
        public long StoryId { get; init; }
        public long UserId { get; init; }
        public string UserRole { get; init; } = null!;
        public IFormFile? ThumbnailFile { get; init; }
    }
}
