using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Enums
{
    public enum UserLogAction : short
    {
        Login = 0,
        Logout = 1,
        Register = 2,
        ResetPassword = 3,
        UpdateProfile = 4,
        FollowStory = 5,
        LockAccount = 6,
        UnlockAccount = 7

    }
}
