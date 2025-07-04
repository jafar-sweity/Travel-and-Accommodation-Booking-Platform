using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Services
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> GenerateInvoiceAsync(Booking booking);
    }
}
