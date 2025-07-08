using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;


namespace TravelAndAccommodationBookingPlatform.Application.Handlers.ReviewHandlers
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReviewCommandHandler(
            IReviewRepository reviewRepository,
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var userRole = _currentUserService.GetUserRole();

            if (userRole != "Guest")
            {
                throw new ForbiddenException(UserMessages.UserNotGuest);
            }

            var hotelExists = await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId);
            if (!hotelExists)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            var review = await _reviewRepository.GetReviewByIdAsync(request.ReviewId, request.HotelId, userId);
            if (review == null)
                throw new NotFoundException(ReviewMessages.ReviewNotFoundForUserAndHotel);

            await _reviewRepository.RemoveAsync(request.ReviewId);

            var totalRating = await _reviewRepository.GetHotelRatingAsync(request.HotelId);
            var reviewCount = await _reviewRepository.GetHotelReviewCountAsync(request.HotelId);

            totalRating -= review.Rating;
            reviewCount--;

            var newRating = reviewCount > 0 ? totalRating / reviewCount : 0;
            await _hotelRepository.UpdateReviewById(request.HotelId, newRating);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
