using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImageAsync(IFormFile image, Guid entityId);
        Task RemoveImageAsync(Guid entityId);
    }
}
