using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.DeleteHotel
{
    public class DeleteHotelCommand : IRequest
    {
        public Guid HotelId { get; init; }
    }
}
