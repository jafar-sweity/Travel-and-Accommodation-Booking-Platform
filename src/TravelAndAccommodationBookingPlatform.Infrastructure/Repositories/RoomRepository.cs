using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Extensions;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Room?> GetRoomByIdAsync(Guid roomClassId, Guid id)
        {
            ArgumentNullException.ThrowIfNull(roomClassId);
            ArgumentNullException.ThrowIfNull(id);
            return await _context.Rooms
                .Include(r => r.RoomClass)
                .FirstOrDefaultAsync(r => r.Id == id && r.RoomClassId == roomClassId);
        }

        public async Task<PaginatedResult<Room>> GetRoomsAsync(PaginatedQuery<Room> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryable = _context.Rooms.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<Room>(pagedItems, paginationMetadata);
        }

        public async Task<PaginatedResult<RoomManagementDto>> GetRoomsForManagementAsync(PaginatedQuery<Room> query)
        {
            ArgumentNullException.ThrowIfNull(query);
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var queryable = _context.Rooms.AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
                queryable = queryable.Sort(query.SortByColumn, query.SortDirection);

            var paginationMetadata = await queryable.GetPaginationMetadataAsync(query.PageNumber, query.PageSize);

            var pagedItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .Select(room => new RoomManagementDto
                {
                    Id = room.Id,
                    RoomClassId = room.RoomClassId,
                    IsAvailable = !room.Bookings.Any(b => b.CheckInDate >= currentDate && b.CheckOutDate <= currentDate),
                    Number = room.Number,
                    CreatedAt = room.CreatedAt,
                    ModifiedAt = room.UpdatedAt
                })
                .AsNoTracking()
                .ToListAsync();

            return new PaginatedResult<RoomManagementDto>(pagedItems, paginationMetadata);
        }

        public async Task<Room?> GetRoomWithRoomClassByIdAsync(Guid roomId)
        {
            ArgumentNullException.ThrowIfNull(roomId);
            return await _context.Rooms
                .Include(r => r.RoomClass)
                .ThenInclude(rc => rc.Hotel)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }
    }
}
