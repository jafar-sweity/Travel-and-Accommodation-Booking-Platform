using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands
{
    public class DeleteReviewCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid ReviewId { get; init; }
    }
}
