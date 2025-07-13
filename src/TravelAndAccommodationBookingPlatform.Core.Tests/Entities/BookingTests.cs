using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class BookingTests
    {
        private readonly Fixture _fixture;

        public BookingTests()
        {

            _fixture = new Fixture();

            // Handle circular references
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Use custom DateOnly generator (✅ FIXED HERE)
            _fixture.Customizations.Add(new DateOnlyGenerator());

        }

        [Fact]
        public void Booking_Should_Inherit_From_EntityBase()
        {
            // Arrange & Act
            var booking = new Booking();

            // Assert
            booking.Should().BeAssignableTo<EntityBase>();
        }

        [Fact]
        public void Booking_Should_Initialize_Collections()
        {
            // Arrange & Act
            var booking = new Booking();

            // Assert
            booking.Rooms.Should().NotBeNull();
            booking.Rooms.Should().BeEmpty();
            booking.Invoice.Should().NotBeNull();
            booking.Invoice.Should().BeEmpty();
        }

        [Fact]
        public void Booking_Should_Set_And_Get_Properties_Correctly()
        {
            // Arrange
            var guestId = _fixture.Create<Guid>();
            var hotelId = _fixture.Create<Guid>();
            var totalPrice = _fixture.Create<decimal>();
            var checkInDate = DateOnly.FromDateTime(_fixture.Create<DateTime>());
            var checkOutDate = DateOnly.FromDateTime(_fixture.Create<DateTime>());
            var bookingDate = DateOnly.FromDateTime(_fixture.Create<DateTime>());
            var guestRemarks = _fixture.Create<string>();
            var paymentType = _fixture.Create<PaymentType>();

            // Act
            var booking = new Booking
            {
                GuestId = guestId,
                HotelId = hotelId,
                TotalPrice = totalPrice,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                BookingDate = bookingDate,
                GuestRemarks = guestRemarks,
                PaymentType = paymentType
            };

            // Assert
            booking.GuestId.Should().Be(guestId);
            booking.HotelId.Should().Be(hotelId);
            booking.TotalPrice.Should().Be(totalPrice);
            booking.CheckInDate.Should().Be(checkInDate);
            booking.CheckOutDate.Should().Be(checkOutDate);
            booking.BookingDate.Should().Be(bookingDate);
            booking.GuestRemarks.Should().Be(guestRemarks);
            booking.PaymentType.Should().Be(paymentType);
        }

        [Fact]
        public void Booking_Should_Allow_Null_GuestRemarks()
        {
            // Arrange & Act
            var booking = new Booking
            {
                GuestRemarks = null
            };

            // Assert
            booking.GuestRemarks.Should().BeNull();
        }

        [Fact]
        public void Booking_Should_Allow_Adding_Rooms()
        {
            // Arrange
            var booking = new Booking();
            var room1 = _fixture.Create<Room>();
            var room2 = _fixture.Create<Room>();

            // Act
            booking.Rooms.Add(room1);
            booking.Rooms.Add(room2);

            // Assert
            booking.Rooms.Should().HaveCount(2);
            booking.Rooms.Should().Contain(room1);
            booking.Rooms.Should().Contain(room2);
        }

        [Fact]
        public void Booking_Should_Allow_Adding_Invoice_Details()
        {
            // Arrange
            var booking = new Booking();
            var invoice1 = _fixture.Create<InvoiceDetail>();
            var invoice2 = _fixture.Create<InvoiceDetail>();

            // Act
            booking.Invoice.Add(invoice1);
            booking.Invoice.Add(invoice2);

            // Assert
            booking.Invoice.Should().HaveCount(2);
            booking.Invoice.Should().Contain(invoice1);
            booking.Invoice.Should().Contain(invoice2);
        }

        [Theory]
        [InlineData(PaymentType.PayPal)]
        [InlineData(PaymentType.BankTransfer)]
        [InlineData(PaymentType.MasterCard)]
        [InlineData(PaymentType.Visa)]
        public void Booking_Should_Accept_Valid_PaymentType(PaymentType paymentType)
        {
            // Arrange & Act
            var booking = new Booking
            {
                PaymentType = paymentType
            };

            // Assert
            booking.PaymentType.Should().Be(paymentType);
        }

        [Fact]
        public void Booking_Should_Handle_Navigation_Properties()
        {
            // Arrange
            var guest = _fixture.Create<User>();
            var hotel = _fixture.Create<Hotel>();
            var booking = new Booking();

            // Act
            booking.Guest = guest;
            booking.Hotel = hotel;

            // Assert
            booking.Guest.Should().Be(guest);
            booking.Hotel.Should().Be(hotel);
        }
    }
}