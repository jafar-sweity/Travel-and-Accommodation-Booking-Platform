using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public Guid RoomClassId { get; set; }
        public string Name { get; set; }
    }
}
