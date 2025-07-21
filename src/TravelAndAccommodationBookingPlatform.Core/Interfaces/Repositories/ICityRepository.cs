using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetTopMostVisitedCitiesAsync(int count);
        Task<PaginatedResult<CityManagementDto>> GetCitiesForAdminAsync(PaginatedQuery<City> PaginatedQuery);
    }
}
