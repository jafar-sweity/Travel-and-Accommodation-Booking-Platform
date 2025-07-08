using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers
{
    public class RetrieveInvoicePdfQueryHandler : IRequestHandler<RetrieveInvoicePdfQuery, byte[]>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly ICurrentUserService _currentUserService;

        public RetrieveInvoicePdfQueryHandler(
            IBookingRepository bookingRepository,
            IPdfGeneratorService pdfGeneratorService,
            ICurrentUserService currentUserService)
        {
            _bookingRepository = bookingRepository;
            _pdfGeneratorService = pdfGeneratorService;
            _currentUserService = currentUserService;
        }

        public async Task<byte[]> Handle(RetrieveInvoicePdfQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var role = _currentUserService.GetUserRole();

            if (role != "Guest")
                throw new ForbiddenException(UserMessages.UserNotGuest);

            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, userId);

            return booking == null
                ? throw new NotFoundException(BookingMessages.BookingNotFound)
                : await _pdfGeneratorService.GenerateInvoiceAsync(booking);
        }
    }
}
