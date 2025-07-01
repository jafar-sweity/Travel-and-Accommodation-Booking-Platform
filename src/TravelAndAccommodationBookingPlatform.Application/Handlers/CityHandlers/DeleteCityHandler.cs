using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.CityHandlers
{
    public class DeleteCityHandler : IRequestHandler<DeleteCityCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelRepository _hotelRepository;

        public DeleteCityHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IHotelRepository hotelRepository)
        {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
            _hotelRepository = hotelRepository;
        }

        public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.GetByIdAsync(request.CityId) ?? throw new NotFoundException(CityMessages.CityNotFound);

            if (await _hotelRepository.ExistsByPredicateAsync(_hotelRepository => _hotelRepository.CityId == request.CityId))
            {
                throw new InvalidOperationException(CityMessages.CityHasLinkedEntities);
            }

            await _cityRepository.RemoveAsync(request.CityId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
