using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class RoomClassRepository : Repository<RoomClass>, IRoomClassRepository
    {
        private readonly AppDbContext _context;

        public RoomClassRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<RoomClass>> GetRoomClassesAsync(PaginatedQuery<RoomClass> query)
        {
            var currentDateTime = DateTime.UtcNow;

            var roomClassesQuery = _context.RoomClasses
                .Include(rc => rc.Discounts.Where(d => currentDateTime >= d.StartDate && currentDateTime < d.EndDate))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SortByColumn))
            {
                roomClassesQuery = roomClassesQuery.OrderBy(
                    $"{query.SortByColumn} {(query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc")}");
            }

            // Fetch paged room classes
            var pagedRoomClasses = await roomClassesQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            // Load galleries separately
            var roomClassIds = pagedRoomClasses.Select(rc => rc.Id).ToList();
            var galleries = await _context.Images
                .Where(img => roomClassIds.Contains(img.EntityId))
                .ToListAsync();

            // Attach galleries
            foreach (var rc in pagedRoomClasses)
            {
                rc.Gallery = galleries.Where(img => img.EntityId == rc.Id).ToList();
            }

            // Get total count
            var totalCount = await roomClassesQuery.CountAsync();

            return new PaginatedResult<RoomClass>(pagedRoomClasses, new PaginationMetadata(totalCount, query.PageNumber, query.PageSize));
        }

        public async Task<bool> ExistsAsync(Expression<Func<RoomClass, bool>> predicate)
        {
            return await _context.RoomClasses.AnyAsync(predicate);
        }

        public async Task<IEnumerable<RoomClass>> GetFeaturedRoomsAsync(int count)
        {
            var currentDateTime = DateTime.UtcNow;

            var featuredDeals = await _context.RoomClasses
                .AsNoTracking()
                .Where(rc => rc.Discounts.Any(d =>
                    d.StartDate <= currentDateTime &&
                    d.EndDate > currentDateTime))
                .Select(rc => new
                {
                    RoomClass = rc,
                    ActiveDiscount = rc.Discounts
                        .Where(d => d.StartDate <= currentDateTime && d.EndDate > currentDateTime)
                        .OrderByDescending(d => d.Percentage)
                        .FirstOrDefault(),
                    Hotel = rc.Hotel,
                    Thumbnail = _context.Images
                        .Where(img => img.EntityId == rc.HotelId)
                        .FirstOrDefault()
                })
                .GroupBy(rcd => rcd.Hotel.Id)
                .Select(g => g
                    .OrderByDescending(rcd => rcd.ActiveDiscount.Percentage)
                    .ThenBy(rcd => rcd.RoomClass.NightlyRate)
                    .FirstOrDefault())
                .Take(count)
                .ToListAsync();

            var result = featuredDeals.Select(d =>
            {
                d.RoomClass.Discounts = new List<Discount> { d.ActiveDiscount };
                d.Hotel.SmallPreview = d.Thumbnail;
                d.RoomClass.Hotel = d.Hotel;
                d.Hotel.City = _context.Cities
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == d.Hotel.CityId);
                return d.RoomClass;
            });

            return result;
        }
    }
}
