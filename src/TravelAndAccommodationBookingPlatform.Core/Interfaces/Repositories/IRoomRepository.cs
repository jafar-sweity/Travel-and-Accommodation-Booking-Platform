using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<PaginatedResult<Room>> GetRoomsAsync(PaginatedQuery<Room> query);
        Task<Room?> GetRoomByIdAsync(Guid roomClassId, Guid id);
        Task<PaginatedResult<RoomManagementDto>> GetRoomsForManagementAsync(PaginatedQuery<Room> query);
        Task<Room?> GetRoomWithRoomClassByIdAsync(Guid roomId);
    }
}
