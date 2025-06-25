using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    interface IRoomClassRepository : IRepository<RoomClass>
    {
        Task<PaginatedResult<RoomClass>> GetRoomClassesAsync(PaginatedQuery<RoomClass> query);
        Task<bool> ExistsAsync(Expression<Func<RoomClass, bool>> predicate);
        Task<IEnumerable<RoomClass>> GetFeaturedRoomsAsync(int count);
    }
}
