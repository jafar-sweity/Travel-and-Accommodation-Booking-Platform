using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.Auth;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        JwtAuthToken CreateTokenForUser(User user);
    }
}
