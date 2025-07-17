using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommand : IRequest
    {
        public Guid DiscountId { get; init; }
        public Guid RoomClassId { get; init; }
    }
}
