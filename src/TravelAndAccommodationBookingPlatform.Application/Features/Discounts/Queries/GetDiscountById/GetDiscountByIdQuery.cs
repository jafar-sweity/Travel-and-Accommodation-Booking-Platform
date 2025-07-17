using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQuery : IRequest<DiscountResponseDto>
    {
        public Guid DiscountId { get; init; }
        public Guid RoomClassId { get; init; }
    }
}
