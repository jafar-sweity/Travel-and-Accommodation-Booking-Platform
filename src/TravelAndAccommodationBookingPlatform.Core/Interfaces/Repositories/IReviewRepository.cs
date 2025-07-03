using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<PaginatedResult<Review>> GetReviewsAsync(PaginatedQuery<Review> query);
        Task<Review?> GetReviewByIdAsync(Guid hotelId, Guid reviewId);
        Task<Review?> GetReviewByIdAsync(Guid reviewId, Guid hotelId, Guid guestId);
        Task<int> GetHotelReviewCountAsync(Guid hotelId);
        Task<int> GetHotelRatingAsync(Guid hotelId);
    }
}
