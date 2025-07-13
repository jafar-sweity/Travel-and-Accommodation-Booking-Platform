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

            var mostVisitedCitiesQuery =
               from booking in _context.Bookings
               group booking by booking.Hotel.CityId into grouped
               orderby grouped.Count() descending
               select new { CityId = grouped.Key, VisitCount = grouped.Count() };

            var mostVisitedCitiesWithThumbnails = await mostVisitedCitiesQuery
                .Take(count)
                .Join(
                    _context.Cities.AsNoTracking(),
                    g => g.CityId,
                    c => c.Id,
                    (g, c) => new { City = c, g.VisitCount }
                )
                .GroupJoin(
                    _context.Images.AsNoTracking(),
                    cityWithVisit => cityWithVisit.City.Id,
                    img => img.EntityId,
                    (cityWithVisit, images) => new
                    {
                        City = cityWithVisit.City,
                        VisitCount = cityWithVisit.VisitCount,
                        SmallPreview = images.FirstOrDefault()
                    }
                )
                .ToListAsync();

            return mostVisitedCitiesWithThumbnails.Select(c =>
            {
                c.City.SmallPreview = c.SmallPreview;
                return c.City;
            });

        }
    }
}
