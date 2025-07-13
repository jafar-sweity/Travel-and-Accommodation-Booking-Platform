using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Application.Handlers.BookingHandlers;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;


namespace TravelAndAccommodationBookingPlatform.Application.Tests.Commands.Bookings
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly IFixture _fixture;

        private readonly Mock<IBookingRepository> _mockBookingRepository;
        private readonly Mock<IHotelRepository> _mockHotelRepository;
        private readonly Mock<IRoomRepository> _mockRoomRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateBookingCommandHandler _handler;

        public CreateBookingCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Register<DateOnly>(() => DateOnly.FromDateTime(DateTime.UtcNow.Date));

            _mockBookingRepository = _fixture.Freeze<Mock<IBookingRepository>>();
            _mockHotelRepository = _fixture.Freeze<Mock<IHotelRepository>>();
            _mockRoomRepository = _fixture.Freeze<Mock<IRoomRepository>>();
            _mockUserRepository = _fixture.Freeze<Mock<IUserRepository>>();
            _mockCurrentUserService = _fixture.Freeze<Mock<ICurrentUserService>>();
            _mockUnitOfWork = _fixture.Freeze<Mock<IUnitOfWork>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();

            _handler = new CreateBookingCommandHandler(
                _fixture.Freeze<Mock<IPdfGeneratorService>>().Object,
                _mockBookingRepository.Object,
                _mockCurrentUserService.Object,
                _mockHotelRepository.Object,
                _mockRoomRepository.Object,
                _mockUserRepository.Object,
                _fixture.Freeze<Mock<IEmailService>>().Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnBookingResponseDto_WhenRequestIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var hotelId = Guid.NewGuid();
            var roomId = Guid.NewGuid();

            var command = _fixture.Build<CreateBookingCommand>()
                .With(c => c.HotelId, hotelId)
                .With(c => c.RoomId, new List<Guid> { roomId })
                .With(c => c.CheckInDate, DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                .With(c => c.CheckOutDate, DateOnly.FromDateTime(DateTime.Now.AddDays(3)))
                .With(c => c.PaymentMethod, PaymentType.MasterCard)
                .With(c => c.GuestRemarks, "Non-smoking room")
                .Create();

            _mockCurrentUserService.Setup(us => us.GetUserId()).Returns(userId);
            _mockCurrentUserService.Setup(us => us.GetUserRole()).Returns("Guest");

            var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
            _mockUserRepository.Setup(ur => ur.GetByIdAsync(userId)).ReturnsAsync(user);

            var hotel = _fixture.Build<Hotel>().With(h => h.Id, hotelId).Create();
            _mockHotelRepository.Setup(hr => hr.GetByIdAsync(hotelId)).ReturnsAsync(hotel);

            var room = _fixture.Build<Room>()
                .With(r => r.Id, roomId)
                .With(r => r.RoomClass, new RoomClass
                {
                    HotelId = hotelId,
                    NightlyRate = 100,
                    Discounts = new List<Discount> { new Discount { Percentage = 10 } }
                })
                .Create();
            _mockRoomRepository.Setup(rr => rr.GetRoomWithRoomClassByIdAsync(roomId)).ReturnsAsync(room);

            _mockBookingRepository.Setup(br => br.ExistsByPredicateAsync(It.IsAny<Expression<Func<Booking, bool>>>())).ReturnsAsync(false);

            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, Guid.NewGuid())
                .With(b => b.GuestId, userId)
                .With(b => b.TotalPrice, 180)
                .Create();

            _mockBookingRepository.Setup(br => br.AddAsync(It.IsAny<Booking>())).ReturnsAsync(booking);

            var expectedResponse = _fixture.Build<BookingResponseDTO>()
                .With(dto => dto.Id, booking.Id)
                .With(dto => dto.TotalPrice, booking.TotalPrice)
                .Create();
            _mockMapper.Setup(m => m.Map<BookingResponseDTO>(booking)).Returns(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedResponse.Id);
            result.TotalPrice.Should().Be(expectedResponse.TotalPrice);
            _mockBookingRepository.Verify(br => br.AddAsync(It.IsAny<Booking>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
