using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly AppDbContext _context;

        public DiscountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Discount?> GetDiscountByIdAsync(Guid id, Guid roomClassId)
        {
            return await _context.Discounts.AsNoTracking()
                          .Where(r => r.Id == id && r.RoomClassId == roomClassId)
                          .FirstOrDefaultAsync();
        }

        public async Task<PaginatedResult<Discount>> GetDiscountsAsync(PaginatedQuery<Discount> query)
        {
            IQueryable<Discount> filteredQuery = _context.Discounts;

            if (query.FilterExpression != null)
                filteredQuery = filteredQuery.Where(query.FilterExpression);

            if (!string.IsNullOrWhiteSpace(query.SortByColumn))
            {
                string sortOrder = query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc";
                filteredQuery = filteredQuery.OrderBy($"{query.SortByColumn} {sortOrder}");
            }

            var totalItemCount = await filteredQuery.CountAsync();

            // Apply pagination
            var skip = (query.PageNumber - 1) * query.PageSize;
            var pageData = await filteredQuery.Skip(skip).Take(query.PageSize).ToListAsync();

            var metadata = new PaginationMetadata(totalItemCount, query.PageNumber, query.PageSize);

            return new PaginatedResult<Discount>(pageData, metadata);
        }
    }
}
