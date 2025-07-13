using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<HotelSearchDto>> FindHotelsAsync(PaginatedQuery<Hotel> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Hotels
                .Include(h => h.City)
                .Include(h => h.RoomClasses)
                .AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.OrderBy($"{query.SortByColumn} {(query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc")}");

            var totalCount = await queryable.CountAsync();

            var pagedItems = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(hotel => new HotelSearchDto
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    StarRating = hotel.StarRating,
                    ReviewsRating = hotel.ReviewsRating,
                    NightlyRate = hotel.RoomClasses.Min(rc => rc.NightlyRate),
                    Description = hotel.BriefDescription
                })
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<HotelSearchDto>(pagedItems, new PaginationMetadata(totalCount, query.PageNumber, query.PageSize));
        }

        public async Task<PaginatedResult<HotelManagementDto>> GetHotelsForManagementPageAsync(PaginatedQuery<Hotel> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Hotels
                .Include(h => h.City)
                .Include(h => h.Owner)
                .Include(h => h.RoomClasses)
                .AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.OrderBy($"{query.SortByColumn} {(query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc")}");

            var totalCount = await queryable.CountAsync();

            var pagedItems = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(hotel => new HotelManagementDto
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    StarRating = hotel.StarRating,
                    NumberOfRooms = hotel.RoomClasses.Count,
                    CreatedAt = hotel.CreatedAt,
                    UpdatedAt = hotel.UpdatedAt,
                    Owner = hotel.Owner
                })
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<HotelManagementDto>(pagedItems, new PaginationMetadata(totalCount, query.PageNumber, query.PageSize));
        }

        public async Task UpdateHotelRatingAsync(Guid id, double newRating)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return;

            hotel.ReviewsRating = newRating;
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewById(Guid hotelId, int newRating)
        {
            var hotel = await _context.Hotels.FindAsync(hotelId);
            if (hotel == null)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            hotel.ReviewsRating = newRating;
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }
    }
}
