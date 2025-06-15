using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    interface IRepository<T> where T : class
    {
        Task<City?> GetByIdAsync(Guid id);
        Task<City> AddAsync(T entity);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
