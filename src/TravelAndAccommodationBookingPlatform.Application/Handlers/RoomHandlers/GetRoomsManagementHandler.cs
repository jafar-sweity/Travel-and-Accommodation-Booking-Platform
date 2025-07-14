using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.DTOs.RoomDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.RoomQueries;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.RoomHandlers
{
    public class GetRoomsManagementHandler : IRequestHandler<GetRoomManagementQuery, PaginatedResult<RoomManagementResponseDto>>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetRoomsManagementHandler(IRoomClassRepository roomClassRepository, IRoomRepository roomRepository, IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RoomManagementResponseDto>> Handle(GetRoomManagementQuery request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId);
            if (!roomClassExists)
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            Expression<Func<Room, bool>> filterExpression = string.IsNullOrEmpty(request.Search)
                 ? _ => true
                 : room => room.Number.Contains(request.Search);

            var query = new PaginatedQuery<Room>(
                filterExpression,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? OrderDirection.Ascending
            );

            var rooms = await _roomRepository.GetRoomsForManagementAsync(query);
            return _mapper.Map<PaginatedResult<RoomManagementResponseDto>>(rooms);
        }
    }
}
