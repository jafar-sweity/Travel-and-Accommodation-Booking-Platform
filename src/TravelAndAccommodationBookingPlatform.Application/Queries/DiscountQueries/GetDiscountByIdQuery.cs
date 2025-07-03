using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.DiscountDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Queries.DiscountQueries
{
    class GetDiscountByIdQuery : IRequest<DiscountResponseDto>
    {
        public Guid DiscountId { get; init; }
        public Guid RoomClassId { get; init; }
    }
}
