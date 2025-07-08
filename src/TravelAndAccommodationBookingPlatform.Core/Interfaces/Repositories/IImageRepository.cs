using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImageAsync(IFormFile image, Guid entityId, string type);

        Task RemoveImageAsync(Guid entityId, string type);
    }
}
