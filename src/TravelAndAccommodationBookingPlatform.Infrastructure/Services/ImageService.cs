using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AWSS3Settings _s3Settings;

        private static readonly string[] AllowedImageFormats = { ".jpg", ".jpeg", ".png" };

        public ImageService(IAmazonS3 s3Client, IOptions<AWSS3Settings> s3Settings)
        {
            _s3Client = s3Client;
            _s3Settings = s3Settings.Value;
        }

        public async Task<Image> StoreAsync(IFormFile image)
        {
            if (image == null || image.Length <= 0)
                throw new ArgumentNullException(nameof(image), "Image is null or empty.");

            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!AllowedImageFormats.Contains(extension))
                throw new ArgumentOutOfRangeException(nameof(image), $"Unsupported format: {extension}");

            var fileName = $"{Guid.NewGuid()}{extension}";
            using var stream = image.OpenReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = _s3Settings.BucketName,
                ContentType = image.ContentType,
            };

            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest);

            return new Image
            {
                Url = $"https://{_s3Settings.BucketName}.s3.{_s3Settings.Region}.amazonaws.com/{fileName}"
            };
        }

        public async Task RemoveAsync(Image image)
        {
            if (image == null || string.IsNullOrWhiteSpace(image.Url))
                throw new ArgumentNullException(nameof(image), "Image is null or invalid.");

            var objectKey = Path.GetFileName(image.Url);

            await _s3Client.DeleteObjectAsync(_s3Settings.BucketName, objectKey);
        }
    }
}
