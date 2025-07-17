using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelFeaturedDeals
{
    public class GetHotelFeaturedDealsQuery : IRequest<IEnumerable<HotelFeaturedDealResponseDto>>
    {
        public int Count { get; init; }
    }
}
