using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<PaginatedResult<Owner>> GetOwnersAsync(PaginatedQuery<Owner> query);
    }
}
