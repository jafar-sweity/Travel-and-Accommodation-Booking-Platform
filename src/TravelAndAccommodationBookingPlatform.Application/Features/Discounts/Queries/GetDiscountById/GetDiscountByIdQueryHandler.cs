using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscountById
{
    class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountResponseDto>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;

        public GetDiscountByIdQueryHandler(
            IDiscountRepository discountRepository,
            IMapper mapper,
            IRoomClassRepository roomClassRepository)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _roomClassRepository = roomClassRepository;
        }

        public async Task<DiscountResponseDto> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
        {
            var discount = await _discountRepository.GetByIdAsync(request.DiscountId) ?? throw new NotFoundException(DiscountMessages.DiscountNotFound);
            var roomClassExists = await _roomClassRepository.ExistsByPredicateAsync(rc => rc.Id == discount.RoomClassId);

            if (!roomClassExists)
            {
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);
            }

            return _mapper.Map<DiscountResponseDto>(discount);
        }
    }
}
