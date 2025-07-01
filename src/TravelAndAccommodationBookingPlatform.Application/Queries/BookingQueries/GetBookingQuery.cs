using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries
{
    class GetBookingQuery : IRequest<PaginatedResult<BookingResponseDTO>>
    {
        public string? SortColumn { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderDirection OrderDirection { get; set; }
    }
}
