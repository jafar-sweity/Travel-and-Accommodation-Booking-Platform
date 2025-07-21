using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Queries.GetReviewsByHotelId
{
    public class GetReviewsByHotelIdQuery : IRequest<PaginatedResult<ReviewResponseDto>>
    {
        public Guid HotelId { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
    }
}
