using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.RoomClassCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.RoomClassHandlers
{
    public class AddGalleryToRoomClassCommandHandler : IRequestHandler<AddGalleryToRoomClassCommand>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddGalleryToRoomClassCommandHandler(IRoomClassRepository roomClassRepository, IImageRepository imageRepository, IUnitOfWork unitOfWork)
        {
            _roomClassRepository = roomClassRepository;
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddGalleryToRoomClassCommand request, CancellationToken cancellationToken)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId))
                throw new NotFoundException(RoomClassMessages.RoomClassNotFound);

            var image = await _imageRepository.UploadImageAsync(request.Image, request.RoomClassId);
            await _unitOfWork.SaveChangesAsync();

        }
    }

}
