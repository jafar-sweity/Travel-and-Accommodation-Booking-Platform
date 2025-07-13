using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands
{
    public class AddCitySmallPreviewImageCommand : IRequest
    {
        public Guid CityId { get; init; }
        public IFormFile Image { get; init; }
    }
}
