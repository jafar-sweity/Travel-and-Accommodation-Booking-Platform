using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<PaginatedResult<Discount>> GetDiscountsAsync(PaginatedQuery<Discount> query);
        Task<Discount?> GetDiscountByIdAsync(Guid id, Guid roomClassId);
    }
}
