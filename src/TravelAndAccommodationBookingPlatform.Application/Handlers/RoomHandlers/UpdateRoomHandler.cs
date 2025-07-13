using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.RoomHandlers
{
    public class UpdateRoomHandler : IRequestHandler<UpdateRoomCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IRoomClassRepository _roomClassRepository;

        public UpdateRoomHandler(
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

        public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsByPredicateAsync(rc => rc.Id == request.RoomClassId);

            if (!roomClassExists)
            {
                throw new KeyNotFoundException("Room class not found.");
            }

            var room = await _roomRepository.GetByIdAsync(request.RoomId) ??
                throw new NotFoundException(RoomMessages.RoomNotFound);

            _mapper.Map(request, room);

            await _roomRepository.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
