using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.FileStorage
{
    public class CloudflareR2Options
    {
        public const string SectionName = "CloudflareR2";
        public string AccountId { get; set; } = null!;
        public string AccessKeyId { get; set; } = null!;
        public string SecretAccessKey { get; set; } = null!;
        public string BucketName { get; set; } = null!;
        public string ServiceUrl { get; set; } = null!;
    }
}
