using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class RoomTests
    {
        private readonly Fixture _fixture;

        public RoomTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Customize<DateOnly>(c => c.FromFactory(() =>
            {
                var year = _fixture.Create<int>() % 100 + 1950;
                var month = _fixture.Create<int>() % 12 + 1;
                var day = _fixture.Create<int>() % 28 + 1;
                return new DateOnly(year, month, day);
            }));

            // Also do this for DateTime if you use both
            _fixture.Customize<DateTime>(c => c.FromFactory(() =>
            {
                var year = _fixture.Create<int>() % 100 + 1950;
                var month = _fixture.Create<int>() % 12 + 1;
                var day = _fixture.Create<int>() % 28 + 1;
                return new DateTime(year, month, day);
            }));
        }

        [Fact]
        public void Room_Should_Allow_Setting_Properties()
        {
            var room = new Room
            {
                RoomClassId = Guid.NewGuid(),
                Number = "101A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            room.RoomClassId.Should().NotBeEmpty();
            room.Number.Should().Be("101A");
            room.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            room.UpdatedAt.Should().HaveValue();
        }

        [Fact]
        public void Room_Should_Allow_Adding_InvoiceDetails()
        {
            var room = new Room();
            var invoice1 = _fixture.Create<InvoiceDetail>();
            var invoice2 = _fixture.Create<InvoiceDetail>();

            room.InvoiceDetail.Add(invoice1);
            room.InvoiceDetail.Add(invoice2);

            room.InvoiceDetail.Should().HaveCount(2);
        }

        [Fact]
        public void Room_Should_Allow_Adding_Bookings()
        {
            var room = new Room();
            var booking1 = _fixture.Create<Booking>();
            var booking2 = _fixture.Create<Booking>();

            room.Bookings.Add(booking1);
            room.Bookings.Add(booking2);

            room.Bookings.Should().HaveCount(2);
        }

        [Fact]
        public void Room_Should_Handle_RoomClass_Navigation_Property()
        {
            var roomClass = _fixture.Create<RoomClass>();
            var room = new Room
            {
                RoomClass = roomClass
            };

            room.RoomClass.Should().Be(roomClass);
        }
    }
}
