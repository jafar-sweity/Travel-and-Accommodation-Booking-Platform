using TravelAndAccommodationBookingPlatform.Core.Models.Email;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest emailRequest);
    }
}
