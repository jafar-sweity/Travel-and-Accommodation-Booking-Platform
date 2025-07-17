using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.CreateCity
{
    public class CreateCityCommand : IRequest<CityResponseDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
