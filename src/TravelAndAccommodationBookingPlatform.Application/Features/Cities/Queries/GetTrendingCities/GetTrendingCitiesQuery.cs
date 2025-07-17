using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetTrendingCities
{
    public class GetTrendingCitiesQuery : IRequest<IEnumerable<TrendingCityResponseDto>>
    {
        public int Count { get; init; }
    }
}
