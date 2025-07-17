using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddGalleryToHotel
{
    public class AddGalleryToHotelCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public IFormFile Image { get; init; }
    }
}
