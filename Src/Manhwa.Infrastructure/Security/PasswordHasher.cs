using Manhwa.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Manhwa.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltRounds = 11;
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);
        }
        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
