﻿using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.CreateHotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Guid>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateHotelCommandHandler(
            IHotelRepository hotelRepository,
            IOwnerRepository ownerRepository,
            ICityRepository cityRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _ownerRepository = ownerRepository;
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            if (!await _ownerRepository.ExistsByPredicateAsync(o => o.Id == request.OwnerId))
                throw new NotFoundException(OwnerMessages.OwnerNotFound);

            if (!await _cityRepository.ExistsByPredicateAsync(c => c.Id == request.CityId))
                throw new NotFoundException(CityMessages.CityNotFound);

            if (request.StarRating < 1 || request.StarRating > 5)
                throw new ValidationException(HotelMessages.InvalidStarRating);

            if (await _hotelRepository.ExistsByPredicateAsync(h => h.Geolocation == request.Geolocation))
                throw new ConflictException(HotelMessages.HotelWithSameLocationExists);

            var hotel = _mapper.Map<Hotel>(request);
            var createdHotel = await _hotelRepository.AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            return createdHotel.Id;
        }
    }
}
