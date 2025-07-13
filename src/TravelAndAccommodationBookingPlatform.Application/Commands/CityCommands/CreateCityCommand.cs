using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.CityDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands
{
    public class CreateCityCommand : IRequest<CityResponseDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
