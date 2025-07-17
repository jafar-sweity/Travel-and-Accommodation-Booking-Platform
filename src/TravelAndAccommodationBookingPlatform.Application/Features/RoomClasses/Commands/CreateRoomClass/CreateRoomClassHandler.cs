using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.CreateRoomClass
{
    public class CreateRoomClassHandler : IRequestHandler<CreateRoomClassCommand, Guid>
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public CreateRoomClassHandler(
            IRoomClassRepository roomClassRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            _roomClassRepository = roomClassRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<Guid> Handle(CreateRoomClassCommand request, CancellationToken cancellationToken)
        {
            var hotelExists = await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId);

            if (!hotelExists)
            {
                throw new NotFoundException(HotelMessages.HotelNotFound);
            }

            var roomClassExists = await _roomClassRepository.ExistsByPredicateAsync(rc => rc.Name == request.Name && rc.HotelId == request.HotelId);

            if (roomClassExists)
            {
                throw new InvalidOperationException(RoomClassMessages.RoomClassNameExistsInHotel);
            }

            var roomClass = _mapper.Map<Core.Entities.RoomClass>(request);
            await _roomClassRepository.AddAsync(roomClass);
            await _unitOfWork.SaveChangesAsync();

            return roomClass.Id;
        }
    }
}
