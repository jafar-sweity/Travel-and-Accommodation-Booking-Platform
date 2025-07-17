using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public Guid RoomClassId { get; set; }
        public string Number { get; set; }
    }
}
