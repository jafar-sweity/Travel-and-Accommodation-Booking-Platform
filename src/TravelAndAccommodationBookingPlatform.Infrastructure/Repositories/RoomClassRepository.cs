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
            var now = DateTime.UtcNow;

            var roomClassesQuery = _context.RoomClasses
                .Include(rc => rc.Discounts.Where(d => d.StartDate <= now && now < d.EndDate))
                .Include(rc => rc.Gallery)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SortByColumn))
            {
                roomClassesQuery = roomClassesQuery.OrderBy($"{query.SortByColumn} {(query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc")}");
            }

            var totalCount = await roomClassesQuery.CountAsync();
            var paged = await roomClassesQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PaginatedResult<RoomClass>(paged, new PaginationMetadata(totalCount, query.PageNumber, query.PageSize));
        }

        public async Task<bool> ExistsAsync(Expression<Func<RoomClass, bool>> predicate)
        {
            return await _context.RoomClasses.AnyAsync(predicate);
        }

        public async Task<IEnumerable<RoomClass>> GetFeaturedRoomsAsync(int count)
        {
            var currentDate = DateTime.UtcNow;


            var roomsWithActiveDiscounts = await _context.RoomClasses
                .Include(rc => rc.Hotel)
                    .ThenInclude(h => h.City)
                .Include(rc => rc.Gallery)
                .Include(rc => rc.Discounts)
                .Where(rc => rc.Discounts.Any(d => d.StartDate <= currentDate && d.EndDate > currentDate))
                .ToListAsync();
            var roomWithBestDiscountPerHotel = roomsWithActiveDiscounts
                   .Select(rc => new
                   {
                       RoomClass = rc,
                       ActiveDiscount = rc.Discounts
                           .Where(d => d.StartDate <= currentDate && d.EndDate > currentDate)
                           .OrderByDescending(d => d.Percentage)
                           .ThenBy(d => rc.NightlyRate)
                           .FirstOrDefault()
                   })
                   .GroupBy(x => x.RoomClass.HotelId)
                   .Select(g => g
                       .OrderByDescending(x => x.ActiveDiscount.Percentage)
                       .ThenBy(x => x.RoomClass.NightlyRate)
                       .First())
                   .OrderByDescending(x => x.ActiveDiscount.Percentage)
                   .ThenBy(x => x.RoomClass.NightlyRate)
                   .Take(count)
                   .ToList();

            foreach (var item in roomWithBestDiscountPerHotel)
            {
                item.RoomClass.Discounts = new List<Discount> { item.ActiveDiscount };
            }

            return roomWithBestDiscountPerHotel.Select(x => x.RoomClass);
        }
    }
}
