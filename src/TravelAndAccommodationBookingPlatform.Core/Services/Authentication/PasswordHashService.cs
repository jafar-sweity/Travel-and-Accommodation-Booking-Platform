using TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Services.Authentication
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
