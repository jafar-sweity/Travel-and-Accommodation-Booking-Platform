using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetHotelRatingAsync(Guid hotelId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return (int)reviews.Average(r => r.Rating);
        }

        public async Task<int> GetHotelReviewCountAsync(Guid hotelId)
        {
            var reviewCount = await _context.Reviews
                .CountAsync(r => r.HotelId == hotelId);

            return reviewCount;
        }

        public async Task<Review?> GetReviewByIdAsync(Guid hotelId, Guid reviewId)
        {
            var review = await _context.Reviews
                .Where(r => r.HotelId == hotelId && r.Id == reviewId)
                .FirstOrDefaultAsync();

            return review;
        }

        public Task<Review?> GetReviewByIdAsync(Guid reviewId, Guid hotelId, Guid guestId)
        {
            return _context.Reviews
                .Where(r => r.Id == reviewId && r.HotelId == hotelId && r.GuestId == guestId)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedResult<Review>> GetReviewsAsync(PaginatedQuery<Review> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Reviews.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .Include(r => r.Guest)
                .Include(r => r.Hotel)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<Review>(pagedItems, paginationMetadata);
        }
    }
}
