using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    interface IUserRepository : IRepository<User>
    {
        Task<User?> AuthenticateUserAsync(string email, string password);
        Task<bool> UserExistsByEmailAsync(string email);

    }
}
