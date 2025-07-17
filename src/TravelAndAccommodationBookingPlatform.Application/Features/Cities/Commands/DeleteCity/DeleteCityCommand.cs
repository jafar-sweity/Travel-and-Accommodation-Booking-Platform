using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.DeleteCity
{
    public class DeleteCityCommand : IRequest
    {
        public Guid CityId { get; set; }
    }
}
