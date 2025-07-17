using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public Guid ReviewId { get; init; }
    }
}
