﻿using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddGalleryToHotel
{
    public class AddGalleryToHotelCommandHandler : IRequestHandler<AddGalleryToHotelCommand>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddGalleryToHotelCommandHandler(IHotelRepository hotelRepository, IImageRepository imageRepository, IUnitOfWork unitOfWork)
        {
            _hotelRepository = hotelRepository;
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddGalleryToHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelExists = await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId);

            if (!hotelExists)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            await _imageRepository.UploadImageAsync(request.Image, request.HotelId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
