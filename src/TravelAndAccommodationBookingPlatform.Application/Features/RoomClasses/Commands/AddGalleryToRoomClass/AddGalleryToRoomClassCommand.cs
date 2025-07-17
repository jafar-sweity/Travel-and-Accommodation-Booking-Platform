using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.AddGalleryToRoomClass
{
    public class AddGalleryToRoomClassCommand : IRequest
    {
        public Guid RoomClassId { get; init; }
        public IFormFile Image { get; init; }
    }
}
