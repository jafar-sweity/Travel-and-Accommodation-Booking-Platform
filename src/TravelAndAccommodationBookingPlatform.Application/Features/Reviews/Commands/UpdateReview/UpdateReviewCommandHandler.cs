using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var userRole = _currentUserService.GetUserRole();

            if (userRole != "Guest")
                throw new ForbiddenException(UserMessages.UserNotGuest);

            if (request.Rating < 1 || request.Rating > 5)
                throw new ValidationException(ReviewMessages.InvalidReviewRating);

            var hotelExists = await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId);
            if (!hotelExists)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            var review = await _reviewRepository.GetReviewByIdAsync(request.ReviewId, request.HotelId, userId);
            if (review == null)
                throw new NotFoundException(ReviewMessages.ReviewNotFoundForUserAndHotel);

            _mapper.Map(request, review);
            review.UpdatedAt = DateTime.Now;

            await _reviewRepository.UpdateAsync(review);

            var totalRating = await _reviewRepository.GetHotelRatingAsync(request.HotelId);
            var reviewCount = await _reviewRepository.GetHotelReviewCountAsync(request.HotelId);
            var newRating = (totalRating + request.Rating) / reviewCount;

            await _hotelRepository.UpdateReviewById(request.HotelId, newRating);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
