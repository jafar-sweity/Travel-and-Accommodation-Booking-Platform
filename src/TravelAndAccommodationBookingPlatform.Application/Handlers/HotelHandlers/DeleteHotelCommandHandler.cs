using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.HotelCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.HotelHandlers
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHotelCommandHandler(IRoomClassRepository roomClassRepository, IHotelRepository hotelRepository, IUnitOfWork unitOfWork)
        {
            _roomClassRepository = roomClassRepository;
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            if (!await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId))
                throw new NotFoundException(HotelMessages.HotelNotFound);

            if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == request.HotelId))
                throw new DependentsExistException(HotelMessages.HotelHasDependents);

            await _hotelRepository.RemoveAsync(request.HotelId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
