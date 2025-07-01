using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.RoomHandlers
{
    class CreateRoomHndler : IRequestHandler<CreateRoomCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;

        public CreateRoomHndler(
            IUnitOfWork unitOfWork,
            IRoomRepository roomRepository,
            IMapper mapper,
            IRoomClassRepository roomClassRepository)
        {
            _unitOfWork = unitOfWork;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _roomClassRepository = roomClassRepository;
        }

        public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsByPredicateAsync(rc => rc.Id == request.RoomClassId);

            if (!roomClassExists)
            {
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);
            }

            var roomExists = await _roomRepository.ExistsByPredicateAsync(r => r.Number == request.Number && r.RoomClassId == request.RoomClassId);

            if (roomExists)
            {
                throw new RoomWithNumberExistsInRoomClassException(RoomClassMessages.DuplicatedRoomNumber);
            }

            var newRoom = _mapper.Map<Core.Entities.Room>(request);

            await _roomRepository.AddAsync(newRoom);
            await _unitOfWork.SaveChangesAsync();
            return newRoom.Id;

        }
    }
}
