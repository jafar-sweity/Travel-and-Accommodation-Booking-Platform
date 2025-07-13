namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class AWSS3Settings
    {
        public string BucketName { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
    }
}
