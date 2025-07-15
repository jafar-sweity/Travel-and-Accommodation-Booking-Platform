using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Hotels
{
    public class GetHotelsPublicRequestDto
    {
        public string? Search { get; set; }
        public string? SortColumn { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderDirection SortDirection { get; set; } = OrderDirection.Ascending;
    }
}
