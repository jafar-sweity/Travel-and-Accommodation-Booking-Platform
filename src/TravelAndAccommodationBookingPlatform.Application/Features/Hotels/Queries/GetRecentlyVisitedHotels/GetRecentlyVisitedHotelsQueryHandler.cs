﻿using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetRecentlyVisitedHotels
{
    public class GetRecentlyVisitedHotelsQueryHandler : IRequestHandler<GetRecentlyVisitedHotelsQuery, IEnumerable<RecentlyVisitedHotelResponseDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetRecentlyVisitedHotelsQueryHandler(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecentlyVisitedHotelResponseDto>> Handle(GetRecentlyVisitedHotelsQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.GetByIdAsync(request.GuestId);

            if (userExists == null)
                throw new NotFoundException(UserMessages.UserNotFound);

            var recentBookings = await _bookingRepository.GetRecentBookingsByGuestIdAsync(request.GuestId, request.Count);

            if (!recentBookings.Any())
                return Enumerable.Empty<RecentlyVisitedHotelResponseDto>();

            return _mapper.Map<IEnumerable<RecentlyVisitedHotelResponseDto>>(recentBookings);
        }
    }
}
