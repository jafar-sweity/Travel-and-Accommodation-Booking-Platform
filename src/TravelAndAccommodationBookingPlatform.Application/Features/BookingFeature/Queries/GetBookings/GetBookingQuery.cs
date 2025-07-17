using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Booking.Queries.GetBookings
{
    public class GetBookingQuery : IRequest<PaginatedResult<BookingResponseDTO>>
    {
        public string? SortColumn { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderDirection OrderDirection { get; set; }
    }
}
