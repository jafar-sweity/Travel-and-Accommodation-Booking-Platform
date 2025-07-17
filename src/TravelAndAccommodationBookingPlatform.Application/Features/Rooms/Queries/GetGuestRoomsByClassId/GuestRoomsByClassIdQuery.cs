using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetGuestRoomsByClassId
{
    public class GuestRoomsByClassIdQuery : IRequest<PaginatedResult<RoomGuestResponseDto>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public Guid RoomClassId { get; init; }
        public DateOnly CheckInDate { get; init; }
        public DateOnly CheckOutDate { get; init; }
    }
}
