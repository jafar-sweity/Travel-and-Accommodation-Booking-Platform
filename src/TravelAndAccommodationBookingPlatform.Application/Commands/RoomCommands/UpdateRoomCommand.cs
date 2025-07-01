using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands
{
    class UpdateRoomCommand : IRequest
    {
        public Guid RoomId { get; init; }
        public Guid RoomClassId { get; init; }
        public string Number { get; init; }
    }
}
