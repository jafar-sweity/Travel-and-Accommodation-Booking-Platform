using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelGuestById
{
    public class GetHotelGuestByIdQuery : IRequest<HotelGuestResponseDto>
    {
        public Guid HotelId { get; init; }
    }
}
