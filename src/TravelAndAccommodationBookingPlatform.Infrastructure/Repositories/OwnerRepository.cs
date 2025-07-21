using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

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
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Owners.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<Owner>(pagedItems, paginationMetadata);
        }
    }
}
