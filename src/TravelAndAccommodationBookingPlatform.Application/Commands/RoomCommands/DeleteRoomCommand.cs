using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands
{
    public class DeleteRoomCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public Guid RoomClassId { get; set; }
    }
}
