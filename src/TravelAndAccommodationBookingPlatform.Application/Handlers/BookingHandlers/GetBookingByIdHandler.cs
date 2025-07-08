using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers
{
    public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingResponseDTO>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetBookingByIdHandler(
            IBookingRepository bookingRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<BookingResponseDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
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

            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, guestId);

            if (booking == null)
            {
                throw new NotFoundException(BookingMessages.BookingNotFound);
            }

            return _mapper.Map<BookingResponseDTO>(booking);
        }
    }
}
