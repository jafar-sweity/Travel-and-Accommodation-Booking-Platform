using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetRoleByNameAsync(string name);
    }
}
