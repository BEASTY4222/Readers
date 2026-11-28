using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Readers.Core.Contracts;

namespace Readers.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 S3Client;
        private readonly string BucketName;
        private readonly string PublicUrl;

        //would really like to put the conctructor in the short form up but idk how to do it with the config and s3client
        public ImageService(IAmazonS3 s3Client, IConfiguration config)
        {
            S3Client = s3Client;
            BucketName = config["CloudflareR2:BucketName"]!;
            PublicUrl = config["CloudflareR2:PublicUrl"]!;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null!;

            // 1. Generate a unique file name
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"authors/{Guid.NewGuid()}{extension}";

            // 2. Create the upload request
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = fileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
                // Crucial settings for Cloudflare R2 compatibility:
                DisablePayloadSigning = true,
                DisableDefaultChecksumValidation = true
            };

            // 3. Upload to R2
            await S3Client.PutObjectAsync(request);

            // 4. Return the public URL
            return $"{PublicUrl}/{fileName}";
        }
    }
}