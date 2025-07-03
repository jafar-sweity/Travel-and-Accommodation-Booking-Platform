using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Application.Commands.ReviewCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.ReviewDtos;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.ReviewHandlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IBookingRepository bookingRepository, IReviewRepository reviewRepository, IHotelRepository hotelRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewResponseDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
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

            var hasBooking = await _bookingRepository.ExistsByPredicateAsync(b => b.HotelId == request.HotelId && b.GuestId == userId);

            if (!hasBooking)
                throw new GuestNotBookedHotelException(BookingMessages.NoBookingsForGuestInHotel);

            var alreadyReviewed = await _reviewRepository.ExistsByPredicateAsync(r => r.HotelId == request.HotelId && r.GuestId == userId);

            if (alreadyReviewed)
                throw new ReviewAlreadyExistsException(ReviewMessages.GuestAlreadyReviewedHotel);

            var totalRating = await _reviewRepository.GetHotelRatingAsync(request.HotelId);
            var reviewCount = await _reviewRepository.GetHotelReviewCountAsync(request.HotelId);

            var newRating = (totalRating + request.Rating) / (reviewCount + 1);

            await _hotelRepository.UpdateReviewById(request.HotelId, newRating);

            var reviewEntity = _mapper.Map<Review>(request);
            reviewEntity.GuestId = userId;
            reviewEntity.CreatedAt = DateTime.Now;

            var createdReview = await _reviewRepository.AddAsync(reviewEntity);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ReviewResponseDto>(createdReview);
        }
    }
}
