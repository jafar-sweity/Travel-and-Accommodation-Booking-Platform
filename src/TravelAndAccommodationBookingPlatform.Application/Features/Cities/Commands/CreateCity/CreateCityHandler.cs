using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.CreateCity
{
    public class CreateCityHandler : IRequestHandler<CreateCityCommand, CityResponseDto>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCityHandler(ICityRepository cityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CityResponseDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            if (await _cityRepository.ExistsByPredicateAsync(c => c.PostOffice == request.PostOffice))
            {
                throw new InvalidOperationException(CityMessages.CityWithPostalCodeExists);
            }

            var newCity = _mapper.Map<City>(request);
            await _cityRepository.AddAsync(newCity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CityResponseDto>(newCity);
        }
    }
}
