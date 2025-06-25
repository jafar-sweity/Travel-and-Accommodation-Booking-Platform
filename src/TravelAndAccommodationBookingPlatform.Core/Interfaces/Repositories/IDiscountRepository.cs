using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<PaginatedResult<Discount>> GetDiscountsAsync(PaginatedQuery<Discount> query);
    }
}
