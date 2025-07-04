using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<CityManagementDto>> GetCitiesForAdminAsync(PaginatedQuery<City> PaginatedQuery)
        {
            ArgumentNullException.ThrowIfNull(PaginatedQuery);
            IQueryable<City> filteredQuery = _context.Cities.Include(c => c.Hotels);

            if (!string.IsNullOrEmpty(PaginatedQuery.SortByColumn))
            {
                filteredQuery = filteredQuery.OrderBy($"{PaginatedQuery.SortByColumn} {(PaginatedQuery.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc")}");
            }
            var totalItemCount = await filteredQuery.CountAsync();

            var skip = (PaginatedQuery.PageNumber - 1) * PaginatedQuery.PageSize;
            var citiesPage = await filteredQuery
                .Skip(skip)
                .Take(PaginatedQuery.PageSize)
                .Select(c => new CityManagementDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Country = c.Country,
                    Region = c.Region,
                    PostOffice = c.PostOffice,
                    TotalHotels = c.Hotels.Count,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
            .ToListAsync();
            var metadata = new PaginationMetadata(totalItemCount, PaginatedQuery.PageNumber, PaginatedQuery.PageSize);

            return new PaginatedResult<CityManagementDto>(citiesPage, metadata);
        }

        public async Task<IEnumerable<City>> GetTopMostVisitedCitiesAsync(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

            var mostVisitedCities = await _context.Bookings
               .GroupBy(b => b.Hotel.CityId)
               .Select(g => new
               {
                   CityId = g.Key,
                   VisitCount = g.Count()
               })
               .OrderByDescending(g => g.VisitCount)
               .Take(count)
               .Join(
                   _context.Cities
                       .Include(c => c.Hotels)
                       .Include(c => c.SmallPreview)
                       .AsNoTracking(),
                   g => g.CityId,
                   c => c.Id,
                   (g, c) => c
               )
               .ToListAsync();

            return mostVisitedCities;

        }
    }
}
