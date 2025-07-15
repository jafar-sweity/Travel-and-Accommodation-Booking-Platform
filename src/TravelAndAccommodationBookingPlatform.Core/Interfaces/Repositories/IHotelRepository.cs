using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task UpdateHotelRatingAsync(Guid id, double newRating);
        Task<PaginatedResult<HotelManagementDto>> GetHotelsForManagementPageAsync(PaginatedQuery<Hotel> query);
        Task<PaginatedResult<HotelSearchDto>> FindHotelsAsync(PaginatedQuery<Hotel> query);
        Task UpdateReviewById(Guid hotelId, int newRating);
        Task<Hotel?> GetHotelByIdAsync(Guid id);
        Task<PaginatedResult<HotelPublicResponseDto>> GetPublicHotelsAsync(PaginatedQuery<Hotel> query);
    }
}
