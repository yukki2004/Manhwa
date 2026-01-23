using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Enums.Notification
{
    public enum NotificationType : short
    {
        System = 0,
        NewChapter = 1,
        AccountWarning = 2,
        StoryAlert = 3,
        LevelUp = 4,
        CommentReply = 5,
        NewComment = 6
    }
}
