using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

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
            IQueryable<Booking> filteredQuery = _context.Bookings;

            if (query.FilterExpression != null)
            {
                filteredQuery = filteredQuery.Where(query.FilterExpression);
            }

            if (!string.IsNullOrEmpty(query.SortByColumn))
            {
                filteredQuery = filteredQuery.OrderBy($"{query.SortByColumn} {(query.SortDirection == OrderDirection.Ascending ? "asc" : "desc")}");
            }

            var totalItemCount = await filteredQuery.CountAsync();

            var skip = (query.PageNumber - 1) * query.PageSize;
            var bookingsPage = await filteredQuery.Skip(skip).Take(query.PageSize).ToListAsync();

            var metadata = new PaginationMetadata(totalItemCount, query.PageNumber, query.PageSize);

            return new PaginatedResult<Booking>(bookingsPage, metadata);
        }

        public async Task<IEnumerable<Booking>> GetRecentBookingsByGuestIdAsync(Guid guestId, int count)
        {
            return await _context.Bookings
                .Where(b => b.GuestId == guestId)
                .OrderByDescending(b => b.CheckInDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
