using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<BookingResponseDTO>
    {
        public IEnumerable<Guid> RoomId { get; init; }
        public Guid HotelId { get; init; }
        public string? GuestRemarks { get; init; }
        public PaymentType PaymentMethod { get; init; }
        public DateOnly CheckInDate { get; init; }
        public DateOnly CheckOutDate { get; init; }
    }
}
