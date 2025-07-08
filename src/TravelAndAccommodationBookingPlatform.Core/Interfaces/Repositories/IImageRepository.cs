using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImageAsync(IFormFile image, Guid entityId);

        Task RemoveImageAsync(Guid entityId);
    }
}
