using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetBookingByIdAsync(Guid id, Guid guestId);
        Task<PaginatedResult<Booking>> GetBookingsAsync(PaginatedQuery<Booking> query);
        Task<IEnumerable<Booking>> GetRecentBookingsByGuestIdAsync(Guid guestId, int count);
    }
}
