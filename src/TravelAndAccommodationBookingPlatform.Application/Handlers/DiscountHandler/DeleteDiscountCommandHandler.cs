using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.DiscountHandler
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomClassRepository _roomClassRepository;

        public DeleteDiscountCommandHandler(
            IDiscountRepository discountRepository,
            IUnitOfWork unitOfWork,
            IRoomClassRepository roomClassRepository)
        {
            _discountRepository = discountRepository;
            _unitOfWork = unitOfWork;
            _roomClassRepository = roomClassRepository;
        }

        public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId);

            if (!roomClassExists)
            {
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);
            }

            var discountExists = await _discountRepository.ExistsByPredicateAsync(d => d.Id == request.DiscountId && d.RoomClassId == request.RoomClassId);

            if (!discountExists)
            {
                throw new NotFoundException(DiscountMessages.DiscountNotFound);
            }

            await _discountRepository.RemoveAsync(request.DiscountId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
