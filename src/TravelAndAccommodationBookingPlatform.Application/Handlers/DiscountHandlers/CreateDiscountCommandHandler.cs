using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.DiscountDtos;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.DiscountHandler
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, DiscountResponseDto>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiscountCommandHandler(
            IDiscountRepository discountRepository,
            IMapper mapper,
            IRoomClassRepository roomClassRepository,
            IUnitOfWork unitOfWork)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _roomClassRepository = roomClassRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DiscountResponseDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            if (request.Percentage < 0 || request.Percentage > 100)
                throw new ValidationException(DiscountMessages.InvalidDiscountPercentage);

            if (request.StartDate > request.EndDate)
                throw new ValidationException("Start date cannot be after end date.");

            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId))
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            if (await _discountRepository.ExistsByPredicateAsync(d => request.EndDate >= d.StartDate && request.StartDate <= d.EndDate))
                throw new ConflictException(DiscountMessages.ConflictingDiscountInterval);

            var discount = _mapper.Map<Core.Entities.Discount>(request);

            discount.CreatedAt = DateTime.Now;

            await _discountRepository.AddAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DiscountResponseDto>(discount);
        }
    }
}
