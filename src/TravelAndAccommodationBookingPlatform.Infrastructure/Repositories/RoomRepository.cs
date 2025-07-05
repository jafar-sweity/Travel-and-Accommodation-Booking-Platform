using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;

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

            IQueryable<Room> roomQuery = _context.Rooms.Include(r => r.RoomClass);

            if (query.FilterExpression != null)
                roomQuery = roomQuery.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
            {
                string direction = query.SortDirection == Core.Enums.OrderDirection.Ascending ? "asc" : "desc";
                roomQuery = roomQuery.OrderBy($"{query.SortByColumn} {direction}");
            }

            var total = await roomQuery.CountAsync();
            var rooms = await roomQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var metadata = new PaginationMetadata(total, query.PageNumber, query.PageSize);
            return new PaginatedResult<Room>(rooms, metadata);
        }

        public async Task<PaginatedResult<RoomManagementDto>> GetRoomsForManagementAsync(PaginatedQuery<Room> query)
        {
            ArgumentNullException.ThrowIfNull(query);
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            IQueryable<Room> queryable = _context.Rooms
                .Include(r => r.RoomClass)
                .ThenInclude(rc => rc.Hotel)
                .AsQueryable();

            if (query.FilterExpression != null)
                queryable = queryable.Where(query.FilterExpression);

            if (!string.IsNullOrEmpty(query.SortByColumn))
            {
                if (query.SortByColumn == "Number" && query.SortDirection == OrderDirection.Ascending)
                    queryable = queryable.OrderBy(r => r.Number);
                else if (query.SortByColumn == "Number" && query.SortDirection == OrderDirection.Descending)
                    queryable = queryable.OrderByDescending(r => r.Number);
            }

            int totalItemCount = await queryable.CountAsync();

            var roomsPage = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(room => new RoomManagementDto
                {
                    Id = room.Id,
                    RoomClassId = room.RoomClassId,
                    IsAvailable = !room.Bookings.Any(b =>
                        b.CheckInDate <= currentDate && b.CheckOutDate >= currentDate),
                    Number = room.Number,
                    CreatedAt = room.CreatedAt,
                    ModifiedAt = room.UpdatedAt
                })
                .ToListAsync();

            var metadata = new PaginationMetadata(totalItemCount, query.PageNumber, query.PageSize);

            return new PaginatedResult<RoomManagementDto>(roomsPage, metadata);
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
