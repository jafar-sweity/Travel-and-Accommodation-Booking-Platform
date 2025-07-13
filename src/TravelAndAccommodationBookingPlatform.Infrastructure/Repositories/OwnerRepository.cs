using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Owner>> GetOwnersAsync(PaginatedQuery<Owner> query)
        {
            IQueryable<Owner> filteredQuery = _context.Owners;

            if (query.FilterExpression != null)
                filteredQuery = filteredQuery.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
            {
                var sortDirection = query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc";
                filteredQuery = filteredQuery.OrderBy($"{query.SortByColumn} {sortDirection}");
            }

            var totalItemCount = await filteredQuery.CountAsync();

            var skip = (query.PageNumber - 1) * query.PageSize;
            var ownersPage = await filteredQuery.Skip(skip).Take(query.PageSize).ToListAsync();

            var metadata = new PaginationMetadata(totalItemCount, query.PageNumber, query.PageSize);

            return new PaginatedResult<Owner>(ownersPage, metadata);
        }
    }
}
