using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, PaginatedResult<BookingResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetBookingQueryHandler(
            IMapper mapper,
            IBookingRepository bookingRepository,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<BookingResponseDTO>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var guestId = _currentUserService.GetUserId();

            if (guestId == Guid.Empty)
            {
                throw new UnauthorizedAccessException(UserMessages.UserNotFound);
            }

            var role = _currentUserService.GetUserRole();

            if (role != "Guest")
            {
                throw new UnauthorizedAccessException(UserMessages.UserNotGuest);
            }

            var filterExpression = (Expression<Func<Booking, bool>>)(b => b.GuestId == guestId);

            var paginatedQuery = new PaginatedQuery<Booking>(
                filterExpression,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection != default ? request.OrderDirection : Core.Enums.OrderDirection.Ascending
            );

            var bookings = await _bookingRepository.GetBookingsAsync(paginatedQuery);

            return _mapper.Map<PaginatedResult<BookingResponseDTO>>(bookings);
        }
    }
}
