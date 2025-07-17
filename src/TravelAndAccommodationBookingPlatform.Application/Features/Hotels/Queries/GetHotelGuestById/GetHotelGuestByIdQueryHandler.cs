using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelGuestById
{
    public class GetHotelGuestByIdQueryHandler : IRequestHandler<GetHotelGuestByIdQuery, HotelGuestResponseDto>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetHotelGuestByIdQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }
        public async Task<HotelGuestResponseDto> Handle(GetHotelGuestByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            return hotel == null ? throw new NotFoundException(HotelMessages.HotelNotFound) : _mapper.Map<HotelGuestResponseDto>(hotel);
        }
    }
}
