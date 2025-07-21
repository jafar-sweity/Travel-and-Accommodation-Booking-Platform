using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetRoomsManagement
{
    public class GetRoomManagementQuery : IRequest<PaginatedResult<RoomManagementResponseDto>>
    {
        public Guid RoomClassId { get; init; }
        public OrderDirection? OrderDirection { get; init; }
        public string? Search { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
