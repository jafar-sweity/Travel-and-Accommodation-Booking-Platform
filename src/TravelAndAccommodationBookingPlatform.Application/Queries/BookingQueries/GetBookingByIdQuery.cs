using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries
{
    public class GetBookingByIdQuery : IRequest<BookingResponseDTO>
    {
        public Guid BookingId { get; set; }
    }
}
