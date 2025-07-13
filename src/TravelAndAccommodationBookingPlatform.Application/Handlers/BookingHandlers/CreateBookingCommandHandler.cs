using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;


namespace TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDTO>
    {
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(
            IPdfGeneratorService pdfGeneratorService,
            IBookingRepository bookingRepository,
            ICurrentUserService currentUserService,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _pdfGeneratorService = pdfGeneratorService;
            _bookingRepository = bookingRepository;
            _currentUserService = currentUserService;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingResponseDTO> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var userRole = _currentUserService.GetUserRole();
            var userEmail = _currentUserService.GetUserEmail();

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(UserMessages.UserNotFound);

            if (userRole != "Guest")
                throw new ForbiddenException(UserMessages.UserNotGuest);

            var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
            if (hotel == null)
                throw new NotFoundException(HotelMessages.HotelNotFound);

            var rooms = new List<Room>();
            foreach (var roomId in request.RoomId)
            {
                var room = await _roomRepository.GetRoomWithRoomClassByIdAsync(roomId);
                if (room == null)
                    throw new NotFoundException(RoomMessages.RoomNotFound);

                if (room.RoomClass.HotelId != request.HotelId)
                    throw new RoomsNotInHotelException(RoomMessages.RoomNotFound);


                bool isAvailable = await IsRoomAvailableAsync(room, request.CheckInDate, request.CheckOutDate);
                if (!isAvailable)
                    throw new RoomNotAvailableException(RoomMessages.RoomUnavailable(roomId));

                rooms.Add(room);
            }

            var totalPrice = CalculateTotalPrice(rooms, request.CheckInDate, request.CheckOutDate);

            var booking = new Booking
            {
                GuestId = userId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                PaymentType = request.PaymentMethod,
                GuestRemarks = request.GuestRemarks,
                TotalPrice = totalPrice,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                Hotel = hotel,
                Rooms = rooms
            };

            var createdBooking = await _bookingRepository.AddAsync(booking);

            foreach (var room in rooms)
            {
                var discountPercentage = room.RoomClass.Discounts.FirstOrDefault()?.Percentage ?? 0;
                var priceAtReservation = room.RoomClass.NightlyRate;
                var amountAfterDiscount = priceAtReservation * (100 - discountPercentage) / 100;

                var invoiceDetail = new InvoiceDetail
                {
                    RoomId = room.Id,
                    BookingId = createdBooking.Id,
                    Booking = createdBooking,
                    RoomClassName = room.RoomClass.Name,
                    RoomNumber = room.Number,
                    DiscountAppliedAtBooking = discountPercentage,
                    PriceAtReservation = priceAtReservation,
                    AmountAfterDiscount = amountAfterDiscount,
                    TotalAmount = amountAfterDiscount
                };

                createdBooking.Invoice.Add(invoiceDetail);
            }

            await _unitOfWork.SaveChangesAsync();



            var pdfBytes = await _pdfGeneratorService.GenerateInvoiceAsync(createdBooking);

            var emailRequest = new EmailRequest(
                ToEmails: new[] { userEmail },
                SubjectLine: "Your Booking Invoice",
                MessageBody: "Thank you for your booking. Please find the invoice attached.",
                FileAttachment: new[]
                {
                        new FileAttachment(
                            FileName: "invoice.pdf",
                            MediaType: "application/pdf",
                            FileContent: pdfBytes
                        )
                }
            );

            await _emailService.SendAsync(emailRequest);

            return _mapper.Map<BookingResponseDTO>(createdBooking);
        }

        private async Task<bool> IsRoomAvailableAsync(Room room, DateOnly checkIn, DateOnly checkOut)
        {
            bool overlappingBookingExists = await _bookingRepository.ExistsByPredicateAsync(b =>
                b.Rooms.Any(r => r.Id == room.Id) &&
                !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut));

            return !overlappingBookingExists;
        }

        private static decimal CalculateTotalPrice(IEnumerable<Room> rooms, DateOnly checkIn, DateOnly checkOut)
        {
            int nights = (checkOut.ToDateTime(TimeOnly.MinValue) - checkIn.ToDateTime(TimeOnly.MinValue)).Days;
            if (nights < 1) nights = 1;

            var totalPerNight = rooms.Sum(room =>
            {
                decimal basePrice = room.RoomClass.NightlyRate;
                var discountPercentage = room.RoomClass.Discounts.FirstOrDefault()?.Percentage ?? 0;
                decimal discountedPrice = basePrice * (100 - discountPercentage) / 100;
                return discountedPrice;
            });

            return totalPerNight * nights;
        }


    }
}

