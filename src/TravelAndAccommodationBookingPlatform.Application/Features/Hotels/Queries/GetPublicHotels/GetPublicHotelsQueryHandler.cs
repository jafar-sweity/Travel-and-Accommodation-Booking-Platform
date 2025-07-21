using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Response;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetPublicHotels
{
    public class GetPublicHotelsQueryHandler : IRequestHandler<GetPublicHotelsQuery, PaginatedResult<HotelPublicResponseDto>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetPublicHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<HotelPublicResponseDto>> Handle(
                GetPublicHotelsQuery request,
                CancellationToken cancellationToken)
        {
            var direction = request.OrderDirection ?? Core.Enums.OrderDirection.Ascending;

            var query = new PaginatedQuery<Hotel>(
                h => true,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                direction);

            var hotels = await _hotelRepository.GetPublicHotelsAsync(query);

            return hotels;
        }
    }
}
