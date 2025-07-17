using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQuery : IRequest<ReviewResponseDto>
    {
        public Guid ReviewId { get; init; }
        public Guid HotelId { get; init; }
    }
}
