using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        public string? ErrorCode { get; }
        public BusinessRuleViolationException(string message, string? errorCode = null)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
