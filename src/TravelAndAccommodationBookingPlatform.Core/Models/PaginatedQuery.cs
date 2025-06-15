using System.Linq.Expressions;

namespace TravelAndAccommodationBookingPlatform.Core.Models
{
    public record PaginatedQuery<TEntity>(
        Expression<Func<TEntity, bool>> FilterExpression,
        string? SortByColumn,
        int PageNumber,
        int PageSize,
        OrderDirection SortDirection = OrderDirection.Ascending
    );

}
