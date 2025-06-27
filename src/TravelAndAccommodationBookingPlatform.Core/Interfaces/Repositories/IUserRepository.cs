using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> AuthenticateUserAsync(string Username, string password);
        Task<bool> UserExistsByEmailAsync(string email);
    }
}
