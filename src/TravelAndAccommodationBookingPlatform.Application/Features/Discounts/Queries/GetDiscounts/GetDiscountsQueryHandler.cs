using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQueryHandler : IRequestHandler<GetDiscountsQuery, PaginatedResult<DiscountResponseDto>>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;

        public GetDiscountsQueryHandler(
            IDiscountRepository discountRepository,
            IMapper mapper,
            IRoomClassRepository roomClassRepository)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _roomClassRepository = roomClassRepository;
        }

        public async Task<PaginatedResult<DiscountResponseDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
        {
            var existRoomClass = await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId);

            if (!existRoomClass)
            {
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);
            }

            var query = new PaginatedQuery<Core.Entities.Discount>(
                d => d.RoomClassId == request.RoomClassId,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? Core.Enums.OrderDirection.Ascending
            );

            var discounts = await _discountRepository.GetDiscountsAsync(query);
            return _mapper.Map<PaginatedResult<DiscountResponseDto>>(discounts);
        }
    }
}
