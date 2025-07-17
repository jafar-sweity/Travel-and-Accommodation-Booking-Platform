using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.UpdateCity
{
    public class UpdateCityCommand : IRequest
    {
        public Guid CityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
    }
}
