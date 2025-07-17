using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Booking.Queries.RetrieveInvoicePdf
{
    public class RetrieveInvoicePdfQuery : IRequest<byte[]>
    {
        public Guid BookingId { get; set; }
    }
}
