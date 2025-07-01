using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands
{
    class DeleteBookingCommand : IRequest
    {
        public Guid BookingId { get; set; }
    }
}
