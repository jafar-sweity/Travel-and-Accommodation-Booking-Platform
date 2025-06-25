using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    interface IDiscountRepository
    {
        Task<PaginatedResult<Discount>> GetDiscountsAsync(PaginatedQuery<Discount> query);
    }
}
