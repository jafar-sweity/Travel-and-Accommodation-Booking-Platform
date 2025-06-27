using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string Username);
        Task<bool> UserExistsByEmailAsync(string email);
    }
}
