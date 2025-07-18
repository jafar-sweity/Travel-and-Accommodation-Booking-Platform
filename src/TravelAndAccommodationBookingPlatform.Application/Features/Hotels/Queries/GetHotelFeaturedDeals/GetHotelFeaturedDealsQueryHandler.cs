﻿using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelFeaturedDeals
{

    public class GetHotelFeaturedDealsQueryHandler : IRequestHandler<GetHotelFeaturedDealsQuery, IEnumerable<HotelFeaturedDealResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;

        public GetHotelFeaturedDealsQueryHandler(IMapper mapper, IRoomClassRepository roomClassRepository)
        {
            _mapper = mapper;
            _roomClassRepository = roomClassRepository;
        }

        public async Task<IEnumerable<HotelFeaturedDealResponseDto>> Handle(GetHotelFeaturedDealsQuery request, CancellationToken cancellationToken)
        {
            if (request.Count <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Count), "Count must be greater than zero.");

            var featuredDeals = await _roomClassRepository.GetFeaturedRoomsAsync(request.Count);
            return _mapper.Map<IEnumerable<HotelFeaturedDealResponseDto>>(featuredDeals);
        }
    }
}
