using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetRecentlyVisitedHotels
{
    public class GetRecentlyVisitedHotelsQuery : IRequest<IEnumerable<RecentlyVisitedHotelResponseDto>>
    {
        public Guid GuestId { get; init; }
        public int Count { get; init; }
    }
}
