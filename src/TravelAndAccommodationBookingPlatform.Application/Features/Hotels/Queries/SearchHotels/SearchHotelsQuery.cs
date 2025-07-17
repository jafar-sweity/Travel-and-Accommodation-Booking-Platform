using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.SearchHotels
{
    public class SearchHotelsQuery : IRequest<PaginatedResult<HotelSearchResultResponseDto>>
    {
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
        public string? Search { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? CityName { get; init; }
        public int MaxChildrenCapacity { get; set; }
        public int MaxAdultsCapacity { get; set; }
        public int NumberOfRooms { get; init; }
        public int? MinStarRating { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public DateOnly CheckInDate { get; init; }
        public DateOnly CheckOutDate { get; init; }
        public IEnumerable<RoomType> RoomTypes { get; init; }
    }
}
