using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.DeleteRoomClass
{
    public class DeleteRoomClassCommand : IRequest
    {
        public Guid RoomClassId { get; init; }
    }
}
