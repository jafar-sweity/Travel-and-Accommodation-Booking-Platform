using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.AddCitySmallPreviewImage
{
    public class AddCitySmallPreviewImageCommandHandler : IRequestHandler<AddCitySmallPreviewImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCitySmallPreviewImageCommandHandler(IImageRepository imageRepository, ICityRepository cityRepository, IUnitOfWork unitOfWork)
        {
            _imageRepository = imageRepository;
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddCitySmallPreviewImageCommand request, CancellationToken cancellationToken)
        {
            if (!await _cityRepository.ExistsByPredicateAsync(c => c.Id == request.CityId))
                throw new NotFoundException(CityMessages.CityNotFound);

            await _imageRepository.RemoveImageAsync(request.CityId);
            await _imageRepository.UploadImageAsync(request.Image, request.CityId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
