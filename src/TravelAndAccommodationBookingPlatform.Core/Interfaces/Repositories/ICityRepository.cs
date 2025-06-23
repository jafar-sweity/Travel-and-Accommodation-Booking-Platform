using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetTopMostVisitedCitiesAsync(int count);
        Task<PaginatedResult<CityAdminView>> GetCitiesForAdminAsync(PaginatedQuery<City> PaginatedQuery);
    }
}
