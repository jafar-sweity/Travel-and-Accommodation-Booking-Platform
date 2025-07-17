using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<ReviewResponseDto>
    {
        public Guid HotelId { get; init; }
        public int Rating { get; init; }
        public string Content { get; init; }
    }
}
