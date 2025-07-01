using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands
{
    class DeleteCityCommand : IRequest
    {
        public Guid CityId { get; set; }
    }
}
