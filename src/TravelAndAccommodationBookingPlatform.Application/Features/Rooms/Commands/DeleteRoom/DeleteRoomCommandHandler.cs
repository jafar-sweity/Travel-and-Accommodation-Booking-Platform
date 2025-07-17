using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomClassRepository _roomClassRepository;

        public DeleteRoomCommandHandler(
            IUnitOfWork unitOfWork,
            IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            IRoomClassRepository roomClassRepository)
        {
            _unitOfWork = unitOfWork;
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _roomClassRepository = roomClassRepository;
        }

        public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var roomClassExists = await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId);

            if (!roomClassExists)
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomClassId, request.RoomId) ?? throw new NotFoundException(RoomMessages.RoomNotFound);

            var hasBookings = await _bookingRepository.ExistsByPredicateAsync(b => b.Rooms.Any(r => r.Id == request.RoomId));
            if (hasBookings)
                throw new InvalidOperationException(RoomMessages.CannotDeleteRoom);

            await _roomRepository.RemoveAsync(request.RoomId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
