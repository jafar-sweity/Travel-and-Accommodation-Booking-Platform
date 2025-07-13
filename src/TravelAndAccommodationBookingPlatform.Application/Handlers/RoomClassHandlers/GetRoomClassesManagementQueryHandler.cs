using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.DTOs.RoomClassDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.RoomClassQueries;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.RoomClassHandlers
{
    public class GetRoomClassesManagementQueryHandler : IRequestHandler<GetRoomClassesManagementQuery, PaginatedResult<RoomClassManagementResponseDto>>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public GetRoomClassesManagementQueryHandler(
            IRoomClassRepository roomClassRepository,
            IDiscountRepository discountRepository,
            IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RoomClassManagementResponseDto>> Handle(GetRoomClassesManagementQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.RoomClass, bool>> filterExpression =
                rc => string.IsNullOrEmpty(request.Search) || rc.Name.Contains(request.Search);

            var query = new PaginatedQuery<Core.Entities.RoomClass>(
                filterExpression,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? Core.Enums.OrderDirection.Ascending
            );

            var roomClasses = await _roomClassRepository.GetRoomClassesAsync(query);

            var roomClassDtos = _mapper.Map<PaginatedResult<RoomClassManagementResponseDto>>(roomClasses);

            return roomClassDtos;
        }
    }
}
