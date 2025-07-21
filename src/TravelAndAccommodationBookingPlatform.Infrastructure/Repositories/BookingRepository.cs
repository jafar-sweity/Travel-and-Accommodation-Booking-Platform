using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id, Guid guestId)
        {
            return await _context.Bookings
                .Where(b => b.Id == id && b.GuestId == guestId)
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedResult<Booking>> GetBookingsAsync(PaginatedQuery<Booking> query)
        {
            var queryable = _context.Bookings.Include(b => b.Hotel).AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);
            var pagedItems = await queryable.GetPage(query.PageNumber, query.PageSize).AsNoTracking().ToListAsync();

            return new PaginatedResult<Booking>(pagedItems, paginationMetadata);
        }

        public async Task<IEnumerable<Booking>> GetRecentBookingsByGuestIdAsync(Guid guestId, int count)
        {

            var latestBookingPerHotel = await (
                from b in _context.Bookings
                where b.GuestId == guestId
                group b by b.HotelId into g
                select new
                {
                    HotelId = g.Key,
                    LatestBookingId = g.OrderByDescending(b => b.CheckInDate)
                                       .Select(b => b.Id)
                                       .First()
                }
            ).Take(count)
             .ToListAsync();

            if (!latestBookingPerHotel.Any())
                return Enumerable.Empty<Booking>();

            var recentBookings = await _context.Bookings
                .Where(b => latestBookingPerHotel.Select(x => x.LatestBookingId).Contains(b.Id))
                .Include(b => b.Hotel)
                .ThenInclude(h => h.City)
                .ToListAsync();

            var images = await _context.Images
                        .Where(img => latestBookingPerHotel.Select(x => x.LatestBookingId).Contains(img.EntityId))
                        .AsNoTracking()
                        .ToListAsync();


            var bookingsWithImages = recentBookings
                .GroupJoin(
                    images,
                    b => b.Id,
                    img => img.EntityId, 
                    (booking, imageGroup) => new
                    {
                        Booking = booking,
                        SmallPreview = imageGroup.FirstOrDefault()
                    })
                .ToList();

            foreach (var item in bookingsWithImages)
            {
                if (item.SmallPreview != null)
                {
                    item.Booking.Hotel.SmallPreview = item.SmallPreview;
                }
            }

            var orderedBookingIds = latestBookingPerHotel
                .OrderByDescending(x => x.LatestBookingId) 
                .Select(x => x.LatestBookingId);

            return bookingsWithImages
                .OrderBy(b => orderedBookingIds.ToList().IndexOf(b.Booking.Id))
                .Select(x => x.Booking);
        }
    }
}
