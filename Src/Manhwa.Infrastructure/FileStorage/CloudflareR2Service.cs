using Amazon.S3.Model;
using Amazon.S3;
using Manhwa.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.FileStorage
{
    public class CloudflareR2Service : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly CloudflareR2Options _options;

        public CloudflareR2Service(IAmazonS3 s3Client, IOptions<CloudflareR2Options> options)
        {
            _s3Client = s3Client;
            _options = options.Value;
        }

        public async Task<string> UploadAsync(Stream fileStream, string path, string contentType, bool isImmutable = false, CancellationToken ct = default)
        {
            var cacheHeader = isImmutable
        ? "public, max-age=31536000, immutable" // Cho Chapter
        : "public, max-age=0, must-revalidate"; // Cho Avatar
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = path,
                InputStream = fileStream,
                ContentType = contentType,
                DisablePayloadSigning = true,
                Headers = { ["Cache-Control"] = cacheHeader }
            };

            await _s3Client.PutObjectAsync(request, ct);

            return path;
        }

        public async Task DeleteAsync(string key, CancellationToken ct = default)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request, ct);
        }
    }
}
