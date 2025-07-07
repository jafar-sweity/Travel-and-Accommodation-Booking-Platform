using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public Task RemoveAsync(Image image)
        {
            throw new NotImplementedException();
        }

        public Task<Image> StoreAsync(IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}
