﻿using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.DeleteRoomClass
{
    class DeleteRoomClassCommandHandler : IRequestHandler<DeleteRoomClassCommand>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomClassCommandHandler(
            IRoomClassRepository roomClassRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _roomClassRepository = roomClassRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteRoomClassCommand request, CancellationToken cancellationToken)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId))
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            if (await _roomRepository.ExistsByPredicateAsync(r => r.RoomClassId == request.RoomClassId))
                throw new DependentsExistException(RoomClassMessages.ExistingRoomsForRoomClass);

            await _roomClassRepository.RemoveAsync(request.RoomClassId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
