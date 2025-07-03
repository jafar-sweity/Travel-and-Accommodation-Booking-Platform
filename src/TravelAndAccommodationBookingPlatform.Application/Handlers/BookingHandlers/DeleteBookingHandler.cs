using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers
{
    public class DeleteBookingHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteBookingHandler(
            IUnitOfWork unitOfWork,
            IBookingRepository bookingRepository,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _bookingRepository = bookingRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
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

            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, guestId) ?? throw new NotFoundException(BookingMessages.BookingNotFound);

            await _bookingRepository.RemoveAsync(booking.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
