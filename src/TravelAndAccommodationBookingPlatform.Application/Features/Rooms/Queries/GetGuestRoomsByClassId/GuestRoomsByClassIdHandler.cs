﻿using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetGuestRoomsByClassId
{
    class GuestRoomsByClassIdHandler : IRequestHandler<GuestRoomsByClassIdQuery, PaginatedResult<RoomGuestResponseDto>>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GuestRoomsByClassIdHandler(IRoomClassRepository roomClassRepository, IRoomRepository roomRepository, IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RoomGuestResponseDto>> Handle(GuestRoomsByClassIdQuery request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId);
            if (!roomClassExists)
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            Expression<Func<Room, bool>> filterExpression = room => room.RoomClassId == request.RoomClassId &&
                                                                   !room.Bookings.Any(booking =>
                                                                   booking.CheckInDate < request.CheckOutDate &&
                                                                   booking.CheckOutDate > request.CheckInDate);

            var query = new PaginatedQuery<Room>(
                filterExpression,
                null,
                request.PageNumber,
                request.PageSize
            );

            var rooms = await _roomRepository.GetRoomsAsync(query);
            return _mapper.Map<PaginatedResult<RoomGuestResponseDto>>(rooms);
        }
    }
}
