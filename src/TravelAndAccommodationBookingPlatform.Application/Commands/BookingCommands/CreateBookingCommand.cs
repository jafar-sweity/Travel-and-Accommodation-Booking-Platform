using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands
{
    class CreateBookingCommand : IRequest<BookingResponseDTO>
    {
    }
}
