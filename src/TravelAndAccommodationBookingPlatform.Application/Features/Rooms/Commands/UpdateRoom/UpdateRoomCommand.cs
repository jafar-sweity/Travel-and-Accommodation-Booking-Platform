using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.UpdateRoom
{
    public class UpdateRoomCommand : IRequest
    {
        public Guid RoomId { get; init; }
        public Guid RoomClassId { get; init; }
        public string Number { get; init; }
    }
}
