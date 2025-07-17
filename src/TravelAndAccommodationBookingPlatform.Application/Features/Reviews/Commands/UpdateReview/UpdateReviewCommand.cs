using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest
    {
        public Guid ReviewId { get; init; }
        public Guid HotelId { get; init; }
        public int Rating { get; init; }
        public string Content { get; init; }
    }
}
