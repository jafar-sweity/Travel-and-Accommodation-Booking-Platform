using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<CityManagementDto>> GetCitiesForAdminAsync(PaginatedQuery<City> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Cities.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .Select(city => new CityManagementDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    Country = city.Country,
                    PostOffice = city.PostOffice,
                    Region = city.Region,
                    TotalHotels = city.Hotels.Count(),
                    CreatedAt = city.CreatedAt,
                    UpdatedAt = city.UpdatedAt
                })
                .ToListAsync();

            return new PaginatedResult<CityManagementDto>(pagedItems, paginationMetadata);
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
