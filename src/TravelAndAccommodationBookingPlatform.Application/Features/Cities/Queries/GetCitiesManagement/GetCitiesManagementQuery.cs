using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetCitiesManagement
{
    public class GetCitiesManagementQuery : IRequest<PaginatedResult<CityManagementResponseDto>>
    {
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
        public string? Search { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
