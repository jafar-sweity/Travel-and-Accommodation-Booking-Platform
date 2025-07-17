using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommand : IRequest<DiscountResponseDto>
    {
        public Guid RoomClassId { get; init; }
        public decimal Percentage { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
