using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassesManagement
{
    public class GetRoomClassesManagementQuery : IRequest<PaginatedResult<RoomClassManagementResponseDto>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? Search { get; init; }
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
    }
}
