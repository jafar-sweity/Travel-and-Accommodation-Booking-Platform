using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.OwnerDtos;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Queries.OwnerQueries
{
    public class GetOwnersQuery : IRequest<PaginatedResult<OwnerResponseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; init; }
        public OrderDirection SortDirection { get; set; } = OrderDirection.Ascending;
        public string? SortColumn { get; set; }
    }
}
