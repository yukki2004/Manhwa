using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryAvatar
{
    public class UpdateStoryAvatarResponse
    {
        public string Message { get; set; } = "Cập nhật thành công";
        public string NewAvatarUrl { get; set; } = null!;
    }
}
