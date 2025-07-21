using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.HotelDtos;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelsManagement
{
    public class GetHotelManagementQuery : IRequest<PaginatedResult<HotelManagementResponseDto>>
    {
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
        public string? Search { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}