using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
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
            var topBookingIdsQuery = from b in _context.Bookings
                                     where b.GuestId == guestId
                                     group b by b.HotelId into g
                                     let latestBooking = g.OrderByDescending(b => b.CheckInDate).FirstOrDefault()
                                     where latestBooking != null
                                     orderby latestBooking.CheckInDate descending
                                     select latestBooking.Id;

            var topBookingIds = await topBookingIdsQuery.Take(count).ToListAsync();

            if (!topBookingIds.Any()) return Enumerable.Empty<Booking>();

            var recentBookingsWithHotel = await _context.Bookings.Where(b => topBookingIds.Contains(b.Id)).Include(b => b.Hotel).ThenInclude(h => h.City).ToListAsync();

            var bookingsWithImages = recentBookingsWithHotel.GroupJoin(_context.Images.AsNoTracking(),
            b => b.Hotel.Id, img => img.EntityId,
            (booking, images) => new
            {
                Booking = booking,
                SmallPreview = images.FirstOrDefault()
            }).ToList();

            foreach (var item in bookingsWithImages)
            {
                if (item.SmallPreview != null)
                {
                    item.Booking.Hotel.SmallPreview = item.SmallPreview;
                }
            }

            return bookingsWithImages.Select(x => x.Booking)
                .OrderBy(b => topBookingIds.IndexOf(b.Id));
        }
    }
}
