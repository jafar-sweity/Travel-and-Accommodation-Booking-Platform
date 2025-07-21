using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwners
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
