using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands
{
    public class UpdateCityCommand : IRequest
    {
        public Guid CityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
    }
}
