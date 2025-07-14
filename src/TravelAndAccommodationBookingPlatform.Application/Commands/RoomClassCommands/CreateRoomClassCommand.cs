using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.RoomClassCommands
{
    public class CreateRoomClassCommand : IRequest<Guid>
    {
        public Guid HotelId { get; set; }
        public string Name { get; set; }
        public int MaxChildrenCapacity { get; init; }
        public int MaxAdultsCapacity { get; init; }
        public decimal NightlyRate { get; init; }
        public string? Description { get; init; }
        public RoomType TypeOfRoom { get; init; }
    }
}
