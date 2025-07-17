using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.DeleteRoom
{
    public class DeleteRoomCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public Guid RoomClassId { get; set; }
    }
}
