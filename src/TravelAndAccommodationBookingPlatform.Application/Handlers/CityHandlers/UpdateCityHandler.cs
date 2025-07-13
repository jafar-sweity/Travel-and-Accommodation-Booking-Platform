using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.CityHandlers
{
    public class UpdateCityHandler : IRequestHandler<UpdateCityCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCityHandler(ICityRepository cityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            if (await _cityRepository.ExistsByPredicateAsync(c => c.PostOffice == request.PostOffice && c.Id != request.CityId))
            {
                throw new InvalidOperationException(CityMessages.CityWithPostalCodeExists);
            }

            var existingCity = await _cityRepository.GetByIdAsync(request.CityId) ??
                throw new NotFoundException(CityMessages.CityNotFound);

            _mapper.Map(request, existingCity);
            await _cityRepository.UpdateAsync(existingCity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
