using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetPublicHotels
{
    public class GetPublicHotelsQuery : IRequest<PaginatedResult<HotelPublicResponseDto>>
    {
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
        public string? Search { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
