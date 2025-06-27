using System.Linq.Expressions;

namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<bool> ExistsByPredicateAsync(Expression<Func<T, bool>> predicate);
    }
}
