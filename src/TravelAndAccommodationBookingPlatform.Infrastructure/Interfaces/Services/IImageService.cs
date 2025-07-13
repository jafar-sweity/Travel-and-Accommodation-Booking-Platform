using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Services
{
    interface IImageService
    {
        Task<Image> StoreAsync(IFormFile image);
        Task RemoveAsync(Image image);
    }
}
