using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task UpdateHotelRatingAsync(Guid id, double newRating);
        Task<PaginatedResult<HotelManagementDto>> GetHotelsForManagementPageAsync(PaginatedQuery<Hotel> query);
        Task<PaginatedResult<HotelSearchDto>> FindHotelsAsync(PaginatedQuery<Hotel> query);
    }
}
