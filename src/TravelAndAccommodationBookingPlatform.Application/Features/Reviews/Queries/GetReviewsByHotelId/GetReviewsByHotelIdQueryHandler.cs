using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Queries.GetReviewsByHotelId
{
    public class GetReviewsByHotelIdQueryHandler : IRequestHandler<GetReviewsByHotelIdQuery, PaginatedResult<ReviewResponseDto>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetReviewsByHotelIdQueryHandler(
            IReviewRepository reviewRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<ReviewResponseDto>> Handle(GetReviewsByHotelIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _hotelRepository.ExistsByPredicateAsync(h => h.Id == request.HotelId))
                throw new NotFoundException(HotelMessages.HotelNotFound);

            Expression<Func<Review, bool>> filterExpression = r => r.HotelId == request.HotelId;

            var query = new PaginatedQuery<Review>(
                filterExpression,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? OrderDirection.Ascending
            );

            var reviews = await _reviewRepository.GetReviewsAsync(query);
            return _mapper.Map<PaginatedResult<ReviewResponseDto>>(reviews);
        }
    }
}
