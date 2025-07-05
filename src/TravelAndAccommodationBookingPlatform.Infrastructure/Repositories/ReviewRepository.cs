using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

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
            IQueryable<Review> reviewsQuery = _context.Reviews
                            .Include(r => r.Guest)
                            .Include(r => r.Hotel);

            if (query.FilterExpression != null)
            {
                reviewsQuery = reviewsQuery.Where(query.FilterExpression);
            }

            if (!string.IsNullOrEmpty(query.SortByColumn))
            {
                var sortDirection = query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc";
                reviewsQuery = reviewsQuery.OrderBy($"{query.SortByColumn} {sortDirection}");
            }

            var totalCount = await reviewsQuery.CountAsync();

            var pagedReviews = await reviewsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var metadata = new PaginationMetadata(totalCount, query.PageNumber, query.PageSize);
            return new PaginatedResult<Review>(pagedReviews, metadata);
        }
    }
}
