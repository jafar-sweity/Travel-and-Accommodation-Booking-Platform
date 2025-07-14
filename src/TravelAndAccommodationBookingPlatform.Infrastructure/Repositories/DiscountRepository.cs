using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

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
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Discounts.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<Discount>(pagedItems, paginationMetadata);
        }
    }
}
