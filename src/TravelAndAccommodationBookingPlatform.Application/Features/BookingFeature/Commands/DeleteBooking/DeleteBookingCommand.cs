using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest
    {
        public Guid BookingId { get; set; }
    }
}
