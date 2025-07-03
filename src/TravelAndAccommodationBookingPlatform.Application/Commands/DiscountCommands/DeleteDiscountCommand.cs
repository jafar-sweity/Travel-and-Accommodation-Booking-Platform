using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands
{
    public class DeleteDiscountCommand : IRequest
    {
        public Guid DiscountId { get; init; }
        public Guid RoomClassId { get; init; }
    }
}
