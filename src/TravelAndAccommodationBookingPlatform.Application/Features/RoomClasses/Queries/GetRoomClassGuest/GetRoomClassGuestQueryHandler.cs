﻿using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassGuest
{
    class GetRoomClassGuestQueryHandler : IRequestHandler<GetRoomClassGuestQuery, PaginatedResult<RoomClassGuestResponseDto>>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetRoomClassGuestQueryHandler(
            IRoomClassRepository roomClassRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RoomClassGuestResponseDto>> Handle(GetRoomClassGuestQuery request, CancellationToken cancellationToken)
        {
            if (!await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId))
                throw new NotFoundException(HotelMessages.HotelNotFound);

            Expression<Func<RoomClass, bool>> filterExpression = rc => rc.HotelId == request.HotelId;

            var query = new PaginatedQuery<RoomClass>(
                filterExpression,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? OrderDirection.Ascending
            );

            var roomClasses = await _roomClassRepository.GetRoomClassesAsync(query);
            return _mapper.Map<PaginatedResult<RoomClassGuestResponseDto>>(roomClasses);
        }
    }
}
