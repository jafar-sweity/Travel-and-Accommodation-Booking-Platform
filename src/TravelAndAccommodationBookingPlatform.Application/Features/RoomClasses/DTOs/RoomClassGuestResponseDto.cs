using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.DTOs
{
    public class RoomClassGuestResponseDto
    {
        public Guid RoomClassId { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public decimal NightlyRate { get; init; }
        public int MaxChildrenCapacity { get; init; }
        public int MaxAdultsCapacity { get; init; }
        public IEnumerable<string> Gallery { get; init; }
        public DiscountResponseDto? ActiveDiscount { get; init; }
        public RoomType TypeOfRoom { get; init; }
    }
}
