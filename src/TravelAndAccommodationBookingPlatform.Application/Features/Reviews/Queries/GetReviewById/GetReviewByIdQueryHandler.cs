using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewResponseDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public GetReviewByIdQueryHandler(IReviewRepository reviewRepository, IHotelRepository hotelRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<ReviewResponseDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var hotelExists = await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId);

            if (!hotelExists)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            var review = await _reviewRepository.GetReviewByIdAsync(request.HotelId, request.ReviewId);

            return review == null ? throw new NotFoundException(ReviewMessages.ReviewNotFoundForHotel) : _mapper.Map<ReviewResponseDto>(review);
        }
    }
}
