using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<BookingResponseDTO>
    {
        public Guid BookingId { get; set; }
    }
}
